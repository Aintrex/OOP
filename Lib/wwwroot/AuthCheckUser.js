async function checkAuth() {
    try {
        const response = await fetch('/api/users/check-auth', {
            credentials: 'include'
        });

        if (!response.ok) {
            alert('You are not logged in!!! You will be redirectied to the login page');
            window.location.href = 'login.html';
            return;
        }

        const data = await response.json();

        if (data.isAuthenticated) {
            if (data.role === 'user') {
            } else if (data.role === 'admin') {
                alert('You are not user!!! You wil bew redirected to another page')
                window.location.href = 'admin.html';
            } else {
                console.error('Unknown role:', data.role);
                alert('You are not logged in');
                window.location.href = 'login.html';
            }
        } else {
            alert('You are not logged in');
            window.location.href = 'login.html';
        }
    } catch (error) {
        console.error('An error occurred:', error);
    }
}

checkAuth();