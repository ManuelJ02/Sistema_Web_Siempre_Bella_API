// Obtener referencias a los elementos del DOM
const btnRegistrarCompra = document.getElementById('registrarCompra');
const proveedorInput = document.getElementById('proveedor');
const productoInput = document.getElementById('producto');
const cantidadInput = document.getElementById('cantidad');
const precioInput = document.getElementById('precio');
const tablaCompra = document.getElementById('comprasTable').getElementsByTagName('tbody')[0];

// Función para agregar una fila a la tabla
function agregarFilaTabla(proveedor, producto, cantidad, precio) {
    const total = cantidad * precio;
    const nuevaFila = tablaCompra.insertRow();

    // Crear las celdas y agregar los datos
    const celdaProveedor = nuevaFila.insertCell(0);
    const celdaProducto = nuevaFila.insertCell(1);
    const celdaCantidad = nuevaFila.insertCell(2);
    const celdaPrecio = nuevaFila.insertCell(3);
    const celdaTotal = nuevaFila.insertCell(4);

    celdaProveedor.textContent = proveedor;
    celdaProducto.textContent = producto;
    celdaCantidad.textContent = cantidad;
    celdaPrecio.textContent = precio.toFixed(2);
    celdaTotal.textContent = total.toFixed(2);
}

// Evento para registrar la compra
registrarCompra.addEventListener('click', function () {
    const proveedor = proveedorInput.value;
    const producto = productoInput.value;
    const cantidad = parseInt(cantidadInput.value, 10);
    const precio = parseFloat(precioInput.value);

    if (proveedor && producto && cantidad > 0 && precio > 0) {
        // Agregar la fila a la tabla
        agregarFilaTabla(proveedor, producto, cantidad, precio);

        // Limpiar los campos del formulario
        proveedorInput.value = '';
        productoInput.value = '';
        cantidadInput.value = '';
        precioInput.value = '';
    } else {
        alert('Por favor, completa todos los campos correctamente.');
    }
});

function quitarFilaTabla(proveedor, producto, cantidad, precio) {
    const total = cantidad * precio;
    const nuevaFila = tablaCompra.deleteRow();

    // Crear las celdas y agregar los datos
    const celdaProveedor = nuevaFila.deleteCell(0);
    const celdaProducto = nuevaFila.deleteCell(1);
    const celdaCantidad = nuevaFila.deleteCell(2);
    const celdaPrecio = nuevaFila.deleteCell(3);
    const celdaTotal = nuevaFila.deleteCell(4);

    celdaProveedor.textContent = '';
    celdaProducto.textContent = '';
    celdaCantidad.textContent = '';
    celdaPrecio.textContent = '';
    celdaTotal.textContent = '';
}


Comprar.addEventListener('click', function () {
    const proveedor = '';
    const producto = '';
    const cantidad = '';
    const precio = '';

    if (proveedor && producto && cantidad > 0 && precio > 0) {
        // Agregar la fila a la tabla
        quitarFilaTabla(proveedor, producto, cantidad, precio);

        // Limpiar los campos del formulario
        proveedorInput.value = '';
        productoInput.value = '';
        cantidadInput.value = '';
        precioInput.value = '';
    }
});

// Al cargar la página, obtener los proveedores y productos
document.addEventListener('DOMContentLoaded', () => {
    fetchProveedores();
    fetchProductos();
    fetchCompras();
});

// Función para obtener los proveedores desde la API
function fetchProveedores() {
    fetch('https://localhost:7120/api/Proveedor', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            const proveedorSelect = document.getElementById('proveedor');
            data.forEach(proveedor => {
                const option = document.createElement('option');
                option.value = proveedor.id;
                option.textContent = proveedor.nombre;
                proveedorSelect.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error al obtener proveedores:', error);
        });
}

// Función para obtener los productos desde la API
function fetchProductos() {
    fetch('https://localhost:7120/api/Producto', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            const productoSelect = document.getElementById('producto');
            data.forEach(producto => {
                const option = document.createElement('option');
                option.value = producto.id;
                option.textContent = producto.nombre;
                productoSelect.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error al obtener productos:', error);
        });
}

// Función para registrar una compra
document.getElementById('formCompra').addEventListener('submit', function (event) {
    event.preventDefault();

    const proveedorId = document.getElementById('proveedor').value;
    const productoId = document.getElementById('producto').value;
    const cantidad = document.getElementById('cantidad').value;
    const precio = document.getElementById('precio').value;

    if (!proveedorId || !productoId || !cantidad || !precio) {
        alert('Todos los campos son requeridos.');
        return;
    }

    const compra = {
        proveedorId: proveedorId,
        productoId: productoId,
        cantidad: cantidad,
        precio: precio
    };

    fetch('https://localhost:7120/api/Compra', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(compra)
    })
        .then(response => response.json())
        .then(data => {
            if (data) {
                alert('Compra registrada exitosamente.');
                fetchCompras();  // Actualizar la lista de compras
            } else {
                alert('Error al registrar la compra.');
            }
        })
        .catch(error => {
            console.error('Error al registrar compra:', error);
        });
});

// Función para obtener el historial de compras
function fetchCompras() {
    fetch('https://localhost:7120/api/Compra', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            const comprasTable = document.getElementById('comprasTable').getElementsByTagName('tbody')[0];
            comprasTable.innerHTML = ''; // Limpiar tabla antes de agregar

            data.forEach(compra => {
                const row = comprasTable.insertRow();
                row.innerHTML = `
                <td>${compra.proveedor}</td>
                <td>${compra.producto}</td>
                <td>${compra.cantidad}</td>
                <td>${compra.precio}</td>
                <td>${compra.total}</td>
            `;
            });
        })
        .catch(error => {
            console.error('Error al obtener compras:', error);
        });
}

function ListarPedidos() {
    fech('https://localhost:7120/api/Pedido/Listar')
        .then(response => response.json())
        .then(data => {
            const tabla = document.getElementById('pedidoTabla').getElementsByTagName('tbody')[0];
            tabla.innerHTML = '';
            data.response.forEach(pedido => {
                let row = tabla.insertRow();
                row.innerHTML = ` 
                <td>${pedido.NoPedido}</td>
                    <td>${pedido.Fechaentrega}</td>
                    <td>${pedido.IDEmpleado}</td>
                    <td>${pedido.TotalPedido}</td>
                    <td>${pedido.IDProveedores}</td>
                     
                `;
            });
        })
        .catch(error => console.error('error al obtener los pedidos:', error));
}

function GuardarPedido() {
    document.getElementById('pedidoForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const fechaEntrega  = document.getElementById('fechaEntrga').value;
        const idEmpleado = document.getElementById('idEmpleado').value;
        const totalPedido = document.getElementById('totalPedido').value;
        const idProveedores = document.getElementById('idProveedores').value;

        const pedido = {
            fechaEntrega: Dateparse(fechaEntrega),
            idEmpleado: parseInt(idEmpleado),
            totalPedido: parseFloat(totalPedido),
            idProveedores: parseInt(idProveedores)
        };

        fetch('https://localhost:7120/api/Pedido/Guardar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(pedido)
        })
            .then(response => response.json())
            .then(data => {
                console.log('pedido guardado:', data);
                ListarPedidos();
            })
            .catch(error => console.error('error al guardar el pedido:', error));

    })

}

function ObtenerPedido(idPedido) {
    fetch('https://localhost:7120/api/Pedido/Obtener/${idPedido}')
        .then(response => response.json())
        .then(data => {
            console.log('pedido obtenido:', data);
        })
        .catch(error => console.error('error al obtener el pedido:', error));
}

function ListarDetallePedidos() {
    fech('https://localhost:7120/api/DetallePedido/Listar')
        .then(response => response.json())
        .then(data => {
            const Detalletabla = document.getElementById('DetalleTabla').getElementsByTagName('tbody')[0];
            Detalletabla.innerHTML = '';
            data.response.forEach(detallePedido => {
                let row = Detalletabla.insertRow();
                row.innerHTML = ` 
                <td>${detallePedido.IDDetallePedido}</td>
                    <td>${detallePedido.CantidadProductos}</td>
                    <td>${detallepedido.IDProducto}</td>
                    <td>${detallePedido.SubtotalPedido}</td>
                    <td>${detallePedido.NoPedido}</td>
                     
                `;
            });
        })
        .catch(error => console.error('error al obtener los detalles de pedidos:', error));
}

function GuardarDetallePedido() {
    document.getElementById('detallePedidoForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const cantidadProductos = document.getElementById('cantidadProductos').value;
        const idProducto = document.getElementById('idProducto').value;
        const subtotalPedido = document.getElementById('subtotalPedido').value;
        const noPedido = document.getElementById('noPedido').value;

        const detallepedido = {
            cantidadProductos: parseInt(cantidadProductos),
            idProducto: parseInt(idProducto),
            subtotalPedido: parseFloat(subtotalPedido),
            noPedidido: parseInt(noPedido)
        };

        fetch('https://localhost:7120/api/DetallePedido/Guardar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(detallepedido)
        })
            .then(response => response.json())
            .then(data => {
                console.log('detalle de pedido guardado:', data);
                ListarDetallePedidos();
            })
            .catch(error => console.error('error al guardar el detalle pedido:', error));

    })

}

function ObtenerDetallePedido(idDetallePedido) {
    fetch('https://localhost:7120/api/DetallePedido/Obtener/${idDetallePedido}')
        .then(response => response.json())
        .then(data => {
            console.log('detalle pedido obtenido:', data);
        })
        .catch(error => console.error('error al obtener el detalle del pedido:', error));
}

