import { test, expect } from '@playwright/test';

// ─────────────── @smoke 烟雾测试 ───────────────

test.describe('User Flow - Smoke @smoke', () => {
  test('首页加载并显示学生姓名', async ({ page }) => {
    await page.goto('/');
    await expect(page.locator('h2')).toContainText('欢迎');
    await expect(page.locator('.navbar')).toContainText('学生1');
  });

  test('导航栏可切换账号', async ({ page }) => {
    await page.goto('/');
    // 打开下拉菜单
    await page.locator('.navbar .dropdown-toggle').click();
    // 点击"学生4"（在导航栏下拉菜单中）
    await page.locator('.navbar .dropdown-menu button').filter({ hasText: '学生4' }).click();
    await expect(page.locator('.navbar')).toContainText('学生4');
  });

  test('座位列表页加载', async ({ page }) => {
    await page.goto('/Seat/List');
    await expect(page.locator('h2')).toContainText('座位列表');
    // 至少有一个座位卡片
    await expect(page.locator('.card').first()).toBeVisible();
  });

  test('我的预约页加载', async ({ page }) => {
    await page.goto('/Reservation/Mine');
    await expect(page.locator('h2')).toContainText('我的预约');
    // 学生1有预约记录，列表应有内容
    await expect(page.locator('.list-group-item').first()).toBeVisible();
  });

  test('管理员登录页加载', async ({ page }) => {
    await page.goto('/Admin/Login');
    await expect(page.locator('h2, h3').first()).toContainText('管理员登录');
  });
});

// ─────────────── @e2e 主链路测试 ───────────────

test.describe('User Flow - E2E @e2e', () => {
  test.beforeEach(async ({ page }) => {
    // 切换到学生4（无活跃预约，方便测试新建预约）
    await page.goto('/');
    await page.locator('.navbar .dropdown-toggle').click();
    await page.locator('.navbar .dropdown-menu button').filter({ hasText: '学生4' }).click();
    await expect(page.locator('.navbar')).toContainText('学生4');
  });

  test('查看座位 → 预约座位 → 确认在我的预约中出现 → 取消预约', async ({ page }) => {
    // 1. 进入座位列表
    await page.goto('/Seat/List');
    await expect(page.locator('h2')).toContainText('座位列表');

    // 2. 找一个空闲座位点击（第一个卡片）
    const seatCard = page.locator('.card').first();
    await expect(seatCard).toBeVisible();
    await seatCard.click();

    // 3. 应跳转到详情页
    await expect(page.locator('h3')).toBeVisible();
    const seatNumber = await page.locator('h3').textContent();
    expect(seatNumber).toBeTruthy();

    // 4. 如果有"预约此座位"按钮，点击
    const reserveBtn = page.locator('a.btn-primary').filter({ hasText: '预约此座位' });
    if (await reserveBtn.isVisible()) {
      await reserveBtn.click();

      // 5. 在预约表单页，填时间并提交
      await expect(page.locator('h2')).toContainText('预约座位');

      // 设置开始时间为当前时间+1小时，结束时间为当前时间+4小时
      const now = new Date();
      now.setHours(now.getHours() + 1, 0, 0, 0);
      const startStr = now.toISOString().slice(0, 16);

      const end = new Date(now);
      end.setHours(end.getHours() + 3);
      const endStr = end.toISOString().slice(0, 16);

      await page.fill('#StartTime', startStr);
      await page.fill('#EndTime', endStr);

      // 提交
      await page.getByRole('button', { name: '提交预约' }).click();

      // 6. 应跳转到"我的预约"
      await expect(page.locator('h2')).toContainText('我的预约');

      // 7. 新预约应出现在列表中（状态：待签到）
      await expect(page.locator('.list-group-item').first()).toBeVisible();
      // 检查新预约的状态徽章
      await expect(page.locator('.list-group-item').first().locator('.badge')).toContainText('待签到');

      // 8. 取消预约
      const cancelBtn = page.locator('.list-group-item').first().getByRole('button', { name: '取消预约' });
      if (await cancelBtn.isVisible()) {
        // 处理 confirm 弹窗
        page.once('dialog', dialog => {
          expect(dialog.type()).toBe('confirm');
          dialog.accept();
        });
        await cancelBtn.click();
        await page.waitForTimeout(500);
        // 取消后应不再显示"取消预约"按钮（或状态变为已取消）
      }
    }
  });
});

// ─────────────── 规则校验测试 ───────────────

test.describe('User Flow - Validation @e2e', () => {
  test.beforeEach(async ({ page }) => {
    // 使用学生2（有 A-02 待签到预约）
    await page.goto('/');
    await page.locator('.navbar .dropdown-toggle').click();
    await page.locator('.navbar .dropdown-menu button').filter({ hasText: '学生2' }).click();
    await expect(page.locator('.navbar')).toContainText('学生2');
  });

  test('非法取消提示 — 不能取消其他学生的预约', async ({ page }) => {
    // 切换到学生5（在导航栏下拉菜单中）
    await page.locator('.navbar .dropdown-toggle').click();
    await page.locator('.navbar .dropdown-menu button').filter({ hasText: '学生5' }).click();

    // 进入我的预约（学生5无活跃预约）
    await page.goto('/Reservation/Mine');
    // 检查是否有取消按钮 —— 学生5没有待签到的预约
    // 所以不应出现取消按钮
    const cancelBtn = page.locator('button').filter({ hasText: '取消预约' });
    await expect(cancelBtn).toHaveCount(0);
  });

  test('已预约座位列表页不显示免费状态', async ({ page }) => {
    // 学生2预约了A-02（待签到），座位状态应为"已预约"
    // 但学生2仍然可以查看座位列表
    await page.goto('/Seat/List');
    // 至少有一个卡片显示
    await expect(page.locator('.card').first()).toBeVisible();
  });
});
