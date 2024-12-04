document.addEventListener('DOMContentLoaded', () => {
    fetchProveedores();
});

// Función para obtener la lista de proveedores desde la API
function fetchProveedores() {
    fetch('https://localhost:7120/api/Proveedor', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            const tabla = document.getElementById('proveedoresTable').getElementsByTagName('tbody')[0];
            data.forEach(proveedor => {
                const row = tabla.insertRow();
                row.innerHTML = `
                <td>${proveedor.nombre}</td>
                <td>${proveedor.contacto}</td>
                <td>${proveedor.telefono}</td>
                <td>${proveedor.direccion}</td>
                <td>
                    <button onclick="editarProveedor(${proveedor.id})">Editar</button>
                    <button onclick="eliminarProveedor(${proveedor.id})">Eliminar</button>
                </td>
            `;
            });
        })
        .catch(error => {
            console.error('Error al obtener proveedores:', error);
        });
}

// Función para agregar un nuevo proveedor
document.getElementById('formAgregarProveedor').addEventListener('submit', function (event) {
    event.preventDefault();

    const nombre = document.getElementById('nombreProveedor').value;
    const contacto = document.getElementById('contactoProveedor').value;
    const telefono = document.getElementById('telefonoProveedor').value;
    const direccion = document.getElementById('direccionProveedor').value;

    if (!nombre || !contacto || !telefono || !direccion) {
        alert('Todos los campos son requeridos.');
        return;
    }

    const nuevoProveedor = {
        nombre: nombre,
        contacto: contacto,
        telefono: telefono,
        direccion: direccion
    };

    fetch('https://localhost:7120/api/Proveedor', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(nuevoProveedor)
    })
        .then(response => response.json())
        .then(data => {
            alert('Proveedor agregado exitosamente.');
            fetchProveedores();  // Refrescar proveedores
            document.getElementById('formAgregarProveedor').reset();  // Limpiar formulario
        })
        .catch(error => {
            console.error('Error al agregar proveedor:', error);
        });
});

// Función para editar un proveedor
function editarProveedor(proveedorId) {
    // Obtener información del proveedor por su ID
    fetch(`https://localhost:7120/api/Proveedor/${proveedorId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(proveedor => {
            // Rellenar los campos con la información del proveedor
            document.getElementById('nombreProveedor').value = proveedor.nombre;
            document.getElementById('contactoProveedor').value = proveedor.contacto;
            document.getElementById('telefonoProveedor').value = proveedor.telefono;
            document.getElementById('direccionProveedor').value = proveedor.direccion;

            // Cambiar el botón para actualizar el proveedor
            document.querySelector('button[type="submit"]').textContent = 'Actualizar Proveedor';

            // Cambiar la función de submit para actualizar en lugar de agregar
            document.getElementById('formAgregarProveedor').onsubmit = function (event) {
                event.preventDefault();
                const proveedorActualizado = {
                    nombre: document.getElementById('nombreProveedor').value,
                    contacto: document.getElementById('contactoProveedor').value,
                    telefono: document.getElementById('telefonoProveedor').value,
                    direccion: document.getElementById('direccionProveedor').value
                };

                fetch(`https://localhost:7120/api/Proveedor/${proveedorId}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(proveedorActualizado)
                })
                    .then(response => response.json())
                    .then(data => {
                        alert('Proveedor actualizado exitosamente.');
                        fetchProveedores();  // Refrescar proveedores
                        document.getElementById('formAgregarProveedor').reset();  // Limpiar formulario
                        document.querySelector('button[type="submit"]').textContent = 'Agregar Proveedor';  // Resetear el botón
                    })
                    .catch(error => {
                        console.error('Error al actualizar proveedor:', error);
                    });
            };
        })
        .catch(error => {
            console.error('Error al obtener proveedor:', error);
        });
}

// Función para eliminar un proveedor
function eliminarProveedor(proveedorId) {
    if (confirm('¿Estás seguro de eliminar este proveedor?')) {
        fetch(`https://localhost:7120/api/Proveedor/${proveedorId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                alert('Proveedor eliminado.');
                fetchProveedores();  // Refrescar proveedores
            })
            .catch(error => {
                console.error('Error al eliminar proveedor:', error);
            });
    }
}


function ListarProveedores() {
    fetch('https://localhost:7120/api/Proveedor/Lista')
        .then(response => response.json())

        .then(data => {
            const proveedoresTabla = document.getElementById('proveedoresTabla').getElementsByTagName('tbody')[0];
            proveedoresTabla.innerHTML = '';

            data.response.forEach(proveedor => {
                let row = proveedoresTabla.insertRow();
                row.innerHTML =
                    `
                    <td>${proveedor.IDProveedores}</td>
                    <td>${proveedor.NombreEmpresa}</td>
                    <td>${proveedor.NombreGerenteVentas}</td>
                    <td>${proveedor.Telefono}</td>
                    <td>${proveedor.Email}</td>
                    <td>${proveedor.Direccion}</td>
                    <td>
                        <button onclick= "obtenerProveedor(${proveedor.IDProveedores})">Editar</button>
                        <button onclick= "elimnarProveedor(${proveedor.IDProveedores})">Eliminar</button>
                    </td>
                `;
            });

        })
        .catch(error => console.error('Error al obtener la lista de Proveedores:', error));
}

function InsertarProveedores() {
    document.getElementById('proveedorForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const proveedorId = document.getElementById('proveedorId').value;
        const empresa = document.getElementById('empresa').value;
        const nombre = document.getElementById('nombre').value;
        const telefono = document.getElementById('telefono').value;
        const email = document.getElementById('email').value;
        const direccion = document.getElementById('direccion').value;


        const proveedor =
        {
            NombreEmpresa: empresa,
            NombreGerenteVentas: nombre,
            Telefono: telefono,
            Email: email,
            Direccion: direccion
        };

        let url = 'https://localhost:7120/api/Proveedor/Guardar';
        let metodo = 'POST';

        if (ProveedorId) {
            proveedor.IDProveedores = parseInt(proveedorId);
            url = 'https://localhost:7120/api/Proveedor/Editar';
            metodo = 'PUT';
        }

        fetch(url,
            {
                method: metodo,
                headers:
                {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify(proveedor)
            })

            .then(response => response.json())
            .then(data => {
                console.log('Proveedor guardado:', data);
                ListarProveedores();
            })

            .catch(error => console.error('Error al guardar el Proveedor:', error));
    });

}

function BuscarProveedor(id) {
    fetch('https://localhost:7120/api/Proveedor/Obtener/${id}')

        .then(response => response.json())
        .then(data => {
            const proveedor = data.response;
            documment.getElementById('empresa').value = proveedor.NombreEmpresa;
            documment.getElementById('nombre').value = proveedor.NombreGerenteVentas;
            documment.getElementById('telefono').value = proveedor.Telefono;
            documment.getElementById('email').value = proveedor.Email;
            documment.getElementById('direccion').value = proveedor.Direccion;
            documment.getElementById('proveedorId').value = proveedor.IDProveedores;

        })
        .catch(error => console.error('Error al obtener el proveedor:', error))
}

function EliminarProveedor(id) {
    if (confirm('¿Estas seguro de eliminar este proveedor?')) {
        fetch('https://localhost:7120/api/Proveedor/Obtener/${id}', { method: 'DELETE', })
            .then(response => response.json())
            .then(data => {
                console.log('Proveedor Eliminado:', data);
                ListarProveedores();
            })
            .catch(error => console.error('Error al eliminar el proveedor:', error))
    }
}
