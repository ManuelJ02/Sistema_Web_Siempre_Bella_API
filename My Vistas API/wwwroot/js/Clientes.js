// Al cargar la página, obtenemos la lista de clientes
document.addEventListener('DOMContentLoaded', () => {
    fetchClientes();
});

// Función para obtener la lista de clientes desde la API
function fetchClientes() {
    fetch('https://localhost:7120/api/Cliente', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            const clientesTable = document.getElementById('clientesTable').getElementsByTagName('tbody')[0];
            clientesTable.innerHTML = ''; // Limpiar tabla antes de agregar

            data.forEach(cliente => {
                const row = clientesTable.insertRow();
                row.innerHTML = `
                <td>${cliente.nombre}</td>
                <td>${cliente.email}</td>
                <td>${cliente.telefono}</td>
                <td>${cliente.direccion}</td>
                <td>
                    <button onclick="editarCliente(${cliente.id})">Editar</button>
                    <button onclick="eliminarCliente(${cliente.id})">Eliminar</button>
                </td>
            `;
            });
        })
        .catch(error => {
            console.error('Error al obtener clientes:', error);
        });
}

// Función para registrar un nuevo cliente
document.getElementById('formCliente').addEventListener('submit', function (event) {
    event.preventDefault();

    const nombre = document.getElementById('nombre').value;
    const email = document.getElementById('email').value;
    const telefono = document.getElementById('telefono').value;
    const direccion = document.getElementById('direccion').value;

    if (!nombre || !email || !telefono || !direccion) {
        alert('Todos los campos son requeridos.');
        return;
    }

    const cliente = {
        nombre: nombre,
        email: email,
        telefono: telefono,
        direccion: direccion
    };

    fetch('https://localhost:7120/api/Cliente', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(cliente)
    })
        .then(response => response.json())
        .then(data => {
            if (data) {
                alert('Cliente registrado exitosamente.');
                fetchClientes();  // Actualizar la lista de clientes
            } else {
                alert('Error al registrar cliente.');
            }
        })
        .catch(error => {
            console.error('Error al registrar cliente:', error);
        });
});

// Función para editar un cliente
function editarCliente(id) {
    // Lógica para editar el cliente (podría abrir un formulario de edición)
    alert(`Editar cliente con ID: ${id}`);
}

// Función para eliminar un cliente
function eliminarCliente(id) {
    if (confirm('¿Estás seguro de que quieres eliminar este cliente?')) {
        fetch(`https://localhost:7120/api/Cliente/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data) {
                    alert('Cliente eliminado exitosamente.');
                    fetchClientes();  // Actualizar la lista de clientes
                } else {
                    alert('Error al eliminar cliente.');
                }
            })
            .catch(error => {
                console.error('Error al eliminar cliente:', error);
            });
    }
}


function ListarCliente() {
    fetch('https://localhost:7120/api/Cliente/Lista')
        .then(response => response.json())

        .then(data => {
            const clientesTabla = document.getElementById('clientesTabla').getElementsByTagName('tbody')[0];
            clientesTabla.innerHTML = '';

            data.response.forEach(cliente => {
                let row = clientesTabla.insertRow();
                row.innerHTML =
                    `
                    <td>${cliente.IDCliente}</td>
                    <td>${cliente.CedulaCliente}</td>
                    <td>${cliente.NombreCliente}</td>
                    <td>${cliente.Descuento}</td>
                    <td>${cliente.TelefonoCliente}</td>
                    <td>
                        <button onclick= "obtenerCliente(${cliente.IDCliente})">Editar</button>
                        <button onclick= "elimnarCliente(${cliente.IDCliente})">Eliminar</button>
                    </td>
                `;
            });

        })
        .catch(error => console.error('Error al obtener la lista de Clientes:', error));
}

function InsertarCliente() {
    document.getElementById('clienteForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const clienteId = document.getElementById('clienteId').value;
        const cedula = document.getElementById('cedula').value;
        const nombre = document.getElementById('nombre').value;
        const descuento = document.getElementById('descuento').value;
        const telefono = document.getElementById('telefono').value;

        const cliente =
        {
            CedulaCliente: cedula,
            NombreCliente: nombre,
            Descuento: descuento,
            TelefonoCliente: telefono
        };

        let url = 'https://localhost:7120/api/Cliente/Guardar';
        let metodo = 'POST';

        if (ClienteId) {
            cliente.IDCliente = parseInt(clienteId);
            url = 'https://localhost:7120/api/Cliente/Editar';
            metodo = 'PUT';
        }

        fetch(url,
            {
                method: metodo,
                headers:
                {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify(cliente)
            })

            .then(response => response.json())
            .then(data => {
                console.log('Cliente guardado:', data);
                ListarCliente();
            })

            .catch(error => console.error('Error al guardar el Cliente:', error));
    });

}

function Buscarcliente(id) {
    fetch('https://localhost:7120/api/Cliente/Obtener/${id}')

        .then(response => response.json())
        .then(data => {
            const cliente = data.response;
            documment.getElementById('cedula').value = cliente.CedulaCliente;
            documment.getElementById('nombre').value = cliente.NombreCliente;
            documment.getElementById('descuento').value = cliente.Descuento;
            documment.getElementById('telefono').value = cliente.TelefonoCliente;
            documment.getElementById('clienteId').value = cliente.IDCliente;

        })
        .catch(error => console.error('Error al obtener el cliente:', error))
}

function EliminarCliente(id) {
    if (confirm('¿Estas seguro de eliminar este Cliente?')) {
        fetch('https://localhost:7120/api/Cliente/Obtener/${id}', { method: 'DELETE', })
            .then(response => response.json())
            .then(data => {
                console.log('Cliente Eliminado:', data);
                ListarCliente();
            })
            .catch(error => console.error('Error al eliminar el Cliente:', error))
    }
}

