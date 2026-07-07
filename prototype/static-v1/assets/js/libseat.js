/**
 * LibSeat - 图书馆座位预约系统 静态原型交互脚本
 * 仅用于原型演示，不包含任何业务逻辑
 */

// 体验账号切换下拉
function toggleDropdown(event) {
  event.stopPropagation();
  var dropdowns = document.querySelectorAll('.ls-account-dropdown');
  dropdowns.forEach(function(el) {
    el.classList.toggle('show');
  });
}

// 点击页面其他地方关闭下拉
document.addEventListener('click', function() {
  var dropdowns = document.querySelectorAll('.ls-account-dropdown.show');
  dropdowns.forEach(function(el) {
    el.classList.remove('show');
  });
});

// Toast 通知
function showToast(message, type) {
  type = type || 'success';
  var toast = document.getElementById('adminToast');
  if (!toast) {
    toast = document.createElement('div');
    toast.className = 'ls-toast ' + type;
    toast.id = 'adminToast';
    toast.style.top = '20px';
    document.body.appendChild(toast);
  }
  toast.className = 'ls-toast ' + type;
  toast.textContent = (type === 'success' ? '✅ ' : '❌ ') + message;
  toast.classList.add('show');
  setTimeout(function() {
    toast.classList.remove('show');
  }, 2000);
}

// 模态弹窗
function showModal(id) {
  var el = document.getElementById(id);
  if (el) el.classList.add('show');
}

function closeModal(id) {
  var el = document.getElementById(id);
  if (el) el.classList.remove('show');
}

// 简单的筛选按钮切换
document.addEventListener('DOMContentLoaded', function() {
  document.querySelectorAll('.filter-btn').forEach(function(btn) {
    btn.addEventListener('click', function() {
      var parent = this.closest('.filter-bar');
      if (parent) {
        parent.querySelectorAll('.filter-btn').forEach(function(b) {
          b.classList.remove('active');
        });
      }
      this.classList.add('active');
    });
  });
});
