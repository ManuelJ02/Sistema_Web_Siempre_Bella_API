loginForm.addEventListener('submit', async (event) => {
    event.preventDefault();

    const username = document.getElementById('username').value.trim();
    const password = document.getElementById('password').value.trim();
    const errorMessage = document.getElementById('errorMessage');

    if (!username || !password) {
        errorMessage.textContent = "Por favor, complete todos los campos.";
        errorMessage.style.display = "block";
        return;
    }

    try {
        const response = await fetch('https://localhost:7120/api/Empleado/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ Usuario: username, Contraseña: password }),
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem('authToken', data.token);
            localStorage.setItem('userRole', data.CargoEmpleado);
            window.location.href = 'MenuPrincipal.html';
        } else {
            const errorData = await response.json();
            errorMessage.textContent = errorData.message || "Error al iniciar sesión.";
            errorMessage.style.display = "block";
        }
    } catch (error) {
        errorMessage.textContent = "Error de conexión. Intente nuevamente.";
        errorMessage.style.display = "block";
    }
});

function Ingresar() {
    window.location.href = 'MenuPrincipal.html';

}
