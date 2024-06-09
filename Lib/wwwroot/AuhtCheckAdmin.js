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
            if (data.role === 'admin') {
            } else if (data.role === 'user') {
                alert('You are not admin!!! You will be redirected to another page');
                window.location.href = 'index.html';
            } else {
                console.error('Unknown role:', data.role);
                window.location.href = 'login.html';
            }
        } else {
            window.location.href = 'login.html';
        }
    } catch (error) {
        console.error('An error occurred:', error);
    }
}

checkAuth();