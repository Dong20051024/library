# 图书馆座位预约系统 — 脚本烟雾测试
# 检查关键页面是否可访问、关键内容是否存在
# 使用方式: .\scripts\smoke-test.ps1 [-BaseUrl "http://localhost:62260"]

param(
    [string]$BaseUrl = "http://localhost:62260"
)

$passed = 0
$failed = 0
$results = @()

function Check-Page {
    param([string]$Url, [string]$Name, [string]$ExpectedText)

    try {
        $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 10
        if ($response.StatusCode -eq 200) {
            if ($response.Content -match $ExpectedText) {
                Write-Host "  ✅ $Name — 可达，内容匹配" -ForegroundColor Green
                return "PASS"
            } else {
                Write-Host "  ⚠️ $Name — 200 OK 但未找到预期文本" -ForegroundColor Yellow
                return "WARN (200, no match)"
            }
        } else {
            Write-Host "  ❌ $Name — HTTP $($response.StatusCode)" -ForegroundColor Red
            return "FAIL (HTTP $($response.StatusCode))"
        }
    } catch {
        Write-Host "  ❌ $Name — 请求失败: $_" -ForegroundColor Red
        return "FAIL ($_)"
    }
}

Write-Host "===== 烟雾测试: 图书馆座位预约系统 =====" -ForegroundColor Cyan
Write-Host "基础 URL: $BaseUrl"
Write-Host ""

# ── 用户端页面 ──
Write-Host "── 用户端页面 ──" -ForegroundColor Cyan
$r = Check-Page -Url "$BaseUrl/" -Name "首页" -ExpectedText "欢迎"
$results += @{ Page = "首页"; Result = $r }

$r = Check-Page -Url "$BaseUrl/Seat/List" -Name "座位列表" -ExpectedText "座位列表"
$results += @{ Page = "座位列表"; Result = $r }

$r = Check-Page -Url "$BaseUrl/Reservation/Mine" -Name "我的预约" -ExpectedText "我的预约"
$results += @{ Page = "我的预约"; Result = $r }

# 隐私页（本应用无该页面，跳过）

# ── 管理端页面 ──
Write-Host "── 管理端页面 ──" -ForegroundColor Cyan
$r = Check-Page -Url "$BaseUrl/Admin/Login" -Name "管理员登录" -ExpectedText "管理员登录"
$results += @{ Page = "管理员登录"; Result = $r }

# ── 汇总 ──
Write-Host ""
Write-Host "===== 汇总 =====" -ForegroundColor Cyan
foreach ($r in $results) {
    if ($r.Result -match "^PASS") {
        $passed++
    } else {
        $failed++
    }
}
Write-Host "通过: $passed | 失败: $failed" -ForegroundColor $(if ($failed -eq 0) { "Green" } else { "Red" })
Write-Host ""

if ($failed -gt 0) {
    Write-Host "❌ 烟雾测试未通过！请检查服务是否启动。" -ForegroundColor Red
    exit 1
} else {
    Write-Host "✅ 烟雾测试全部通过。" -ForegroundColor Green
    exit 0
}
