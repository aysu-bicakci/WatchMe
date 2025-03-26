// E-posta kullanılabilirliğini kontrol eden fonksiyon

async function checkEmailAvailability(email) {
  const response = await fetch(`https://localhost:5001/api/Auth/check-email?email=${encodeURIComponent(email)}`);
  const result = await response.json();
  return result.isEmailTaken;
}

// Form Submit Event Listener
document.getElementById('registerForm').addEventListener('submit', async function (e) {
  e.preventDefault();

  const nickname = document.getElementById('nickname').value.trim();
  const email = document.getElementById('email').value.trim();
  const password = document.getElementById('password').value;
  const confirmPassword = document.getElementById('confirmPassword').value;

  // Nickname kontrolü
  if (!nickname) {
      showToast('Nickname is required!', 'error');
      return;
  }

  // E-posta format kontrolü
  const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
  if (!emailPattern.test(email)) {
      showToast('Please enter a valid email address!', 'error');
      return;
  }

  // E-posta kullanılabilirliği kontrolü
  try {
      const isEmailTaken = await checkEmailAvailability(email);
      if (isEmailTaken) {
          showToast('This email is already registered. Please use a different one.', 'error');
          return;
      }
  } catch (err) {
      showToast('Error checking email availability!', 'error');
      return;
  }

  // Şifre kontrolü
  const passwordStrengthPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
  if (!passwordStrengthPattern.test(password)) {
      showToast('Password must be at least 8 characters long, contain at least one uppercase letter, one number, and one special character.', 'error');
      return;
  }

  // Şifrelerin eşleşme kontrolü
  if (password !== confirmPassword) {
      showToast('Passwords do not match!', 'error');
      return;
  }

  // Kullanıcı verileri
  const userData = { nickname, email, password };

  // API'ye kayıt isteği
  try {
      const response = await fetch('https://localhost:5001/api/Auth/register', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(userData),
      });

      if (response.ok) {
          showToast('Registration successful!', 'success');
          setTimeout(() => {
              window.location.href = '/Home/Login';
          }, 2000);
      } else {
          const errorData = await response.json();
          showToast(errorData.message || 'An error occurred!', 'error');
      }
  } catch (error) {
      showToast(`Error: ${error.message}`, 'error');
  }
});