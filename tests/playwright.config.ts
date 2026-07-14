import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './e2e',
  fullyParallel: false,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 1 : 0,
  workers: 1,
  reporter: [
    ['list'],
    ['html', { outputFolder: 'test-report' }]
  ],

  // 公共超时：每个测试最多 60 秒；每个 expect 最多 10 秒
  timeout: 60_000,
  expect: { timeout: 10_000 },

  // 使用系统已安装的 Microsoft Edge（不下载独立浏览器）
  use: {
    channel: 'msedge',
    baseURL: 'http://localhost:62260',
    headless: true,
    screenshot: 'only-on-failure',
    trace: 'retain-on-failure',
  },

  // Web 服务：自动启动 ASP.NET Core 项目
  webServer: {
    command: 'dotnet run --project ../src/LibrarySeatReservation.Web',
    url: 'http://localhost:62260',
    reuseExistingServer: true,
    timeout: 120_000,
  },

  projects: [
    {
      name: 'chromium',
      use: {
        channel: 'msedge',
        viewport: { width: 1280, height: 720 },
      },
    },
    {
      name: 'mobile-chrome',
      use: {
        channel: 'msedge',
        viewport: { width: 390, height: 844 },
      },
    },
  ],
});
