// Toast mesajını göstermek için bir fonksiyon
function showToast(message, type = 'info') {
    const toastContainer = document.getElementById('toast-container');
    const toast = document.createElement('div');
    toast.classList.add('toast', 'show', type); // 'success', 'error', 'info', 'warning'
    toast.textContent = message;
  
    // Toast mesajını ekle
    toastContainer.appendChild(toast);
  
    // Toast mesajını kaldırma (5 saniye sonra)
    setTimeout(() => {
      toast.classList.remove('show');
      setTimeout(() => toast.remove(), 500); // Animasyon sonrası öğeyi tamamen sil
    }, 5000);
  }
  