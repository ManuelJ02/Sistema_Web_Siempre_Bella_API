document.addEventListener('DOMContentLoaded', () => {
    fetchClientes();
    fetchProductos();
});

// Función para obtener clientes desde la API
function fetchClientes() {
    fetch('https://localhost:7120/api/Cliente', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            const selectCliente = document.getElementById('clienteFactura');
            data.forEach(cliente => {
                const option = document.createElement('option');
                option.value = cliente.id;
                option.textContent = cliente.nombre;
                selectCliente.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error al obtener clientes:', error);
        });
}

// Función para obtener productos desde la API
function fetchProductos() {
    fetch('https://localhost:7120/api/Producto', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            const selectProducto = document.getElementById('productoFactura');
            data.forEach(producto => {
                const option = document.createElement('option');
                option.value = producto.id;
                option.textContent = `${producto.nombre} - $${producto.precio}`;
                selectProducto.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error al obtener productos:', error);
        });
}

// Función para agregar un producto a la factura
document.getElementById('formFactura').addEventListener('submit', function (event) {
    event.preventDefault();

    const productoId = document.getElementById('productoFactura').value;
    const cantidad = document.getElementById('cantidadProducto').value;
    const clienteId = document.getElementById('clienteFactura').value;

    if (!productoId || !cantidad || !clienteId) {
        alert('Todos los campos son requeridos.');
        return;
    }

    fetch(`https://localhost:7120/api/Producto/${productoId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(producto => {
            const total = producto.precio * cantidad;
            const facturaItem = {
                productoId: producto.id,
                nombre: producto.nombre,
                cantidad: cantidad,
                precio: producto.precio,
                total: total
            };

            agregarProductoAFactura(facturaItem);
            actualizarTotal(facturaItem.total);
        })
        .catch(error => {
            console.error('Error al obtener producto:', error);
        });
});

// Función para agregar un producto a la factura
let totalFactura = 0;
function agregarProductoAFactura(facturaItem) {
    const tabla = document.getElementById('facturaTable').getElementsByTagName('tbody')[0];
    const row = tabla.insertRow();
    row.innerHTML = `
        <td>${facturaItem.nombre}</td>
        <td>${facturaItem.cantidad}</td>
        <td>$${facturaItem.precio}</td>
        <td>$${facturaItem.total}</td>
        <td><button onclick="eliminarProducto(${facturaItem.productoId}, ${facturaItem.total})">Eliminar</button></td>
    `;
}

// Actualizar total de la factura
function actualizarTotal(totalProducto) {
    totalFactura += totalProducto;
    document.getElementById('totalFactura').textContent = `$${totalFactura.toFixed(2)}`;
}

// Eliminar producto de la factura
function eliminarProducto(productoId, total) {
    const rows = document.getElementById('facturaTable').getElementsByTagName('tbody')[0].rows;
    for (let row of rows) {
        if (row.cells[0].textContent === productoId) {
            row.remove();
            totalFactura -= total;
            document.getElementById('totalFactura').textContent = `$${totalFactura.toFixed(2)}`;
        }
    }
}

// Finalizar factura
document.getElementById('finalizarFactura').addEventListener('click', () => {
    if (totalFactura > 0) {
        alert(`Factura finalizada. Total a pagar: $${totalFactura.toFixed(2)}`);
    } else {
        alert('Debe agregar productos antes de finalizar la factura.');
    }
});


function ListarDetallesFactura() {
    fetch('http://localhost:7120/api/DetalleFactura/Lista')
        .then(response => response.json())
        .then(data => {
            const tabla = document.getElementById('detallefacturatabla').getElementsByTagName('tbody')[0];
            tabla.innerHTML = "";

            data.response.forEach(detalle => {
                let row = tabla.insertRow();
                row.innerHTML = `
                    <td>${detalle.IDDetalleFactura}</td>
                    <td>${detalle.CantProductos}</td>
                    <td>${detalle.SubTotalFactura}</td>
                    <td>${detalle.IVA}</td>
                    <td>${detalle.IDProducto}</td>
                     <td>${detalle.NoFactura}</td>
                    `;

            });
        })
        .catch(error => console.error('Error al obtener los detalles de Factura', error));
}

function InsertarDetalleFactura() {
    document.getElementById('detalleFactura').addEventListener('submit', function (e) {
        e.preventDefault();
        const cantProductos = document.getElementById('cantProductos').value;
        const subTotalFactura = document.getElementById('subTotalFactura').value;
        const iva = document.getElementById('iva').value;
        const idProducto = document.getElementById('idProducto').value;
        const noFactura = document.getElementById('noFactura').value; 

        const detallefactura = {

            CantProductos: parseInt(cantProductos),
            SubTotalFactura: parseFloat(subTotalFactura),
            IVA: parseFloat(iva),
            IDProducto: parseInt(idProducto),
            NoFactura: parseInt(noFactura)
        };
        fetch('https://localhost:7120/api/DetalleFactura/Guardar', {
            method: 'POST',
            headers: {
                'Content-Type':'application/json'
            },
            body: JSON.stringify(detallefactura)
        })
            .then(response => response.json())
            .then(data => {
                console.log('Detalle factura Guardado:', data);
                ListarDetallesFactura();
            })
            .catch(error => console.error('Error al guardar los detalles de Factura', error));
    })
    
}

function ObtenerDetalleFactura(idDetalles) {
    fetch('https://localhost:7120/api/DetalleFactura/Obtener/${idDetalles}')
        .then(response => response.json())
        .then(data => {
            console.log('Detalles de Factura obtenidos:', data);
        })
        .catch(error => console.error('Error al Obtener los detalles de Factura', error));
}


function ListarFactura() {
    fetch('https://localhost:7120/api/Factura/Lista')
        .then(response => response.jason())
        .then(data => {
            const tabla = document.getElementById('facturatabla').getElementsByTagName('tbody')[0];
            tabla.innerHTML = "";

            data.response.forEach(factura => {
                let row = tabla.insertRow();
                row.innerHTML = `
                    <td>${factura.NoFactura}</td>
                    <td>${factura.TotalFactura}</td>
                    <td>${factura.FechaFactura}</td>
                    <td>${factura.IDEmpleado}</td>
                    <td>${factura.IDCliente}</td>
                    `;

            });
        })
        .catch(error => console.error('Error al obtener la Factura', error));
}



function InsertarFactura() {
    document.getElementById('factura').addEventListener('submit', function (e) {
        e.preventDefault();
        const noFactura = document.getElementById('noFactura').value;
        const totalFactura = document.getElementById('totalFactura').value;
        const fechaFactura = document.getElementById('fechaFactura').value;
        const idEmpleado = document.getElementById('idEmpleado').value;
        const idCliente = document.getElementById('idCliente').value;

        const factura = {

            NoFactura: parseInt(noFactura),
            TotalFactura: parseFloat(totalFactura),
            FechaFactura: parseFloat(fechaFactura),
            IDEmpleado: parseInt(idEmpleado),
            IDCliente: parseInt(idCliente)
        };
        fetch('https://localhost:7120/api/Factura/Guardar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(factura)
        })
            .then(response => response.json())
            .then(data => {
                console.log('Factura Guardada:', data);
                ListarFactura();
            })
            .catch(error => console.error('Error al guardar la Factura', error));
    })

}


function ObtenerFactura(idFactura) {
    fetch('https://localhost:7120/api/Factura/Obtener/${idFactura}')
        .then(response => response.json())
        .then(data => {
            console.log('Factura obtenida:', data);
        })
        .catch(error => console.error('Error al Obtener la Factura', error));
}

