document.addEventListener('DOMContentLoaded', function () {
  const loginForm = document.querySelector('form');
  const emailInput = document.getElementById('email');
  const passwordInput = document.getElementById('password');

  loginForm.addEventListener('submit', async function (e) {
    e.preventDefault(); // Geleneksel form gönderimini engelle

    const email = emailInput.value.trim();
    const password = passwordInput.value.trim();

    // Boş giriş kontrolü
    if (!email || !password) {
      showToast("Please fill in both email and password.", 'error');
      return;
    }

    const loginData = {
      email,
      password
    };

    try {
      const response = await fetch('https://localhost:5001/api/Auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginData)
      });

      if (response.ok) {
        const result = await response.json();
        showToast(result.message, 'success');
        window.location.href = "/LoginSuccess";
      } else {
        const error = await response.json();
        showToast(error.message || 'Invalid login credentials.', 'error');
      }
    } catch (error) {
      showToast('An error occurred while logging in. Please try again later.', 'error');
    }
  });
});
