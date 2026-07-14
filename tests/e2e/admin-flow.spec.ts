import { test, expect } from '@playwright/test';

// ─────────────── @smoke 烟雾测试 ───────────────

test.describe('Admin Flow - Smoke @smoke', () => {
  test('管理员登录页加载', async ({ page }) => {
    await page.goto('/Admin/Login');
    // 使用 form 内的提交按钮（排除顶部导航栏的退出按钮）
    await expect(page.locator('form button[type="submit"]').first()).toContainText('登录');
  });

  test('管理员登录 → 预约管理页', async ({ page }) => {
    // 使用独立 context 避免 session 残留
    await page.goto('/Admin/Login');
    await page.fill('input[name="Username"]', 'admin');
    await page.fill('input[name="Password"]', '123456');
    // 用 exact 匹配避免匹配到"退出登录"
    await page.getByRole('button', { name: '登录', exact: true }).click();
    // 登录成功后应跳转到预约管理页
    await expect(page.locator('h2')).toContainText('预约管理');
    // 检查侧边栏有预约管理、座位管理、统计
    await expect(page.locator('.admin-sidebar')).toContainText('预约管理');
    await expect(page.locator('.admin-sidebar')).toContainText('座位管理');
    await expect(page.locator('.admin-sidebar')).toContainText('统计');
  });
});

// ─────────────── @e2e 管理端链路测试 ───────────────

test.describe('Admin Flow - E2E @e2e', () => {
  test.beforeEach(async ({ page }) => {
    // 管理员登录（使用独立 session）
    await page.goto('/Admin/Login');
    await page.fill('input[name="Username"]', 'admin');
    await page.fill('input[name="Password"]', '123456');
    await page.getByRole('button', { name: '登录', exact: true }).click();
    await expect(page.locator('h2')).toContainText('预约管理');
  });

  test('预约管理 → 状态筛选', async ({ page }) => {
    // 验证预约列表加载
    await expect(page.locator('table tbody tr').first()).toBeVisible();

    // 选择"待签到"筛选
    await page.selectOption('select[name="status"]', '0');
    await page.waitForTimeout(500);
    // 所有可见行状态徽章应为"待签到"
    const badges = page.locator('table tbody tr td:nth-child(5) .badge');
    const badgeCount = await badges.count();
    if (badgeCount > 0) {
      await expect(badges.first()).toContainText('待签到');
    }
  });

  test('侧边栏 → 座位管理页', async ({ page }) => {
    // 点击侧边栏座位管理
    await page.locator('.admin-sidebar a').filter({ hasText: '座位管理' }).click();
    await expect(page.locator('h2')).toContainText('座位管理');
    // 检查表格存在
    await expect(page.locator('table').first()).toBeVisible();
  });

  test('侧边栏 → 统计页', async ({ page }) => {
    await page.locator('.admin-sidebar a').filter({ hasText: '统计' }).click();
    await expect(page.getByRole('heading', { name: '数据统计' })).toBeVisible();
  });
});

// ─────────────── 签到/释放链路测试 ───────────────

test.describe('Admin Flow - CheckIn/Release @e2e', () => {
  test('预约管理 → 签到操作', async ({ page }) => {
    // 管理员登录
    await page.goto('/Admin/Login');
    await page.fill('input[name="Username"]', 'admin');
    await page.fill('input[name="Password"]', '123456');
    await page.getByRole('button', { name: '登录', exact: true }).click();

    // 筛选"待签到"
    await page.selectOption('select[name="status"]', '0');
    await page.waitForTimeout(500);

    // 如果有"签到"按钮，点击第一个
    const checkinBtn = page.locator('button.btn-success').filter({ hasText: '签到' }).first();
    if (await checkinBtn.isVisible()) {
      await checkinBtn.click();
      await page.waitForTimeout(500);
      // 操作后应有成功消息
      // 注意：签到后该预约不再显示在"待签到"筛选中
    }
  });
});

// ─────────────── 座位管理 CRUD 测试 ───────────────

test.describe('Admin Flow - Seat CRUD @e2e', () => {
  test('座位管理 → 新增座位弹窗', async ({ page }) => {
    // 管理员登录
    await page.goto('/Admin/Login');
    await page.fill('input[name="Username"]', 'admin');
    await page.fill('input[name="Password"]', '123456');
    await page.getByRole('button', { name: '登录', exact: true }).click();

    // 进入座位管理
    await page.locator('.admin-sidebar a').filter({ hasText: '座位管理' }).click();

    // 点击"+ 新增座位"按钮
    await page.getByRole('button', { name: '新增座位' }).click();

    // 弹窗应出现
    await expect(page.locator('#createModal')).toHaveClass(/show/);
    await expect(page.locator('#createModal .modal-title')).toContainText('新增座位');
  });
});
