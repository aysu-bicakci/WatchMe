document.getElementById('forgot-password-form').addEventListener('submit', function(e) {
    e.preventDefault();
  
    const email = document.getElementById('email').value;
    if (email) {
      const apiUrl = 'https://localhost:5001/api/auth/forgot-password'; // API URL'sini buraya yazdık
  
      const data = { email: email };
  
      fetch(apiUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
      })
      .then(response => response.json())
      .then(data => {
        if (data.message) {
          document.getElementById('success-message').style.display = 'block';
        } else {
          alert('An error occurred. Please try again.');
        }
      })
      .catch(error => {
        alert('An error occurred. Please try again.');
      });
    } else {
      alert('Please enter a valid email address.');
    }
  });