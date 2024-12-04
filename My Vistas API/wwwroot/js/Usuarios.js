function ListarEmpleados()
{
    fetch('https://localhost:7120/api/Empleado/Lista')
        .then(response => response.json())

        .then(data =>
        {
            const empleadosTabla = document.getElementById('empleadosTabla').getElementsByTagName('tbody')[0];
            empleadosTabla.innerHTML = '';

            data.response.forEach(empleado =>
            {
                let row = empleadosTabla.insertRow();
                row.innerHTML =
                `
                    <td>${empleado.IDEmpleado}</td>
                    <td>${empleado.NombreEmpleado}</td>
                    <td>${empleado.CargoEmpleado}</td>
                    <td>${empleado.Contraseña}</td>
                    <td>${empleado.Usuario}</td>
                    <td>
                        <button onclick= "obtenerEmpleado(${empleado.IDEmpleado})">Editar</button>
                        <button onclick= "elimnarEmpleado(${empleado.IDEmpleado})">Eliminar</button>
                    </td>
                `;
            });

        })
        .catch(error => console.error('Error al obtener la lista de Empleados:', error));
}

function InsertarEmpleado()
{
    document.getElementById('empleadoForm').addEventListener('submit', function (e)
    {
        e.preventDefault();

        const empleadoId = document.getElementById('empleadoId').value;
        const nombre = document.getElementById('nombre').value;
        const cargo = document.getElementById('cargo').value;
        const contraseña = document.getElementById('contraseña').value;
        const usuario = document.getElementById('usuario').value;

        const empleado =
        {
            NombreEmpleado: nombre,
            CargoEmpleado: cargo,
            Contraseña: contraseña,
            Usuario: usuario
        };

        let url = 'https://localhost:7120/api/Empleado/Guardar';
        let metodo = 'POST';

        if (EmpleadoId)
        {
            empleado.IDEmpleado = parseInt(empleadoId);
            url = 'https://localhost:7120/api/Empleado/Editar';
            metodo = 'PUT';
        }

        fetch(url,
        {
                method: metodo,
                headers:
                {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify(empleado)
        })

            .then(response => response.json())
            .then(data =>
            {
                console.log('Empleado guardado:', data);
                ListarEmpleados();
            })

            .catch(error => console.error('Error al guardar el empleado:', error));
    });

}

function BuscarEmpleado(id)
{
    fetch('https://localhost:7120/api/Empleado/Obtener/${id}') 

        .then(response => response.json())
        .then(data => {
            const empleado = data.response;
            documment.getElementById('nombre').value = empleado.NombreEmpleado;
            documment.getElementById('cargo').value = empleado.CargoEmpleado;
            documment.getElementById('contraseña').value = empleado.ContraseñaEmpleado;
            documment.getElementById('usuario').value = empleado.UsuarioEmpleado;
            documment.getElementById('empleadoId').value = empleado.IDEmpleado;

        })
        .catch(error => console.error('Error al obtener el usuario:', error))
}

function EliminarEmpleado(id) {
    if (confirm('¿Estas seguro de eliminar este usuario?')) {
        fetch('https://localhost:7120/api/Empleado/Obtener/${id}', { method:'DELETE',})
            .then(response => response.json())
            .then(data => {
                console.log('Usuario Eliminado:', data);
                ListarEmpleados();
            })
            .catch(error => console.error('Error al eliminar el usuario:', error))
    }
}