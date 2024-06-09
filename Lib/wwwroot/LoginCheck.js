async function checkAuth() {
    try {
        const response = await fetch('/api/users/check-auth', {
            credentials: 'include'
        });

        if (!response.ok) {

            return;
        }

        const data = await response.json();

        if (data.isAuthenticated) {
            if (data.role === 'admin') {
                alert('You are logged in');
                window.location.href = 'admin.html';
            } else if (data.role === 'user') {
                window.location.href = 'index.html';
            } else {
                console.error('Unknown role:', data.role);
            }
        }
    } catch (error) {
        console.error('An error occurred:', error);
    }
}

checkAuth();