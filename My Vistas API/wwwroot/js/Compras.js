// Función para obtener todos los detalles del pedido
async function obtenerDetallesPedido() {
    try {
        const response = await fetch('https://localhost:7120/api/DetallePedido/Lista', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',  // Asegúrate de que el contenido sea JSON
            }
        });

        if (!response.ok) {
            throw new Error('Error al obtener la lista de detalles del pedido');
        }

        const datos = await response.json();
        console.log('Detalles del pedido:', datos); // Muestra los datos de la lista
    } catch (error) {
        console.error('Error:', error);
        alert('Error al obtener los detalles del pedido');
    }
}

// Llamar la función para obtener la lista de detalles
obtenerDetallesPedido();

// Función para obtener un detalle específico de un pedido por ID
async function obtenerDetallePedido(idDetallePedido) {
    try {
        const response = await fetch(`https://localhost:7120/api/DetallePedido/Obtener/${idDetallePedido}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',  // Asegúrate de que el contenido sea JSON
            }
        });

        if (!response.ok) {
            throw new Error('Error al obtener el detalle del pedido');
        }

        const detalle = await response.json();
        console.log('Detalle del pedido:', detalle); // Muestra el detalle del pedido obtenido
    } catch (error) {
        console.error('Error:', error);
        alert('Error al obtener el detalle del pedido');
    }
}

// Llamar la función para obtener un detalle específico
obtenerDetallePedido(1);  // Reemplaza "1" con el ID del detalle que deseas obtener

// Datos del detalle de pedido a guardar
const datosDetalle = {
    CantidadProductos: 2,  // Ejemplo de cantidad de productos
    IDProducto: 1,         // Ejemplo de ID de producto
    SubtotalPedido: 100.00,  // Ejemplo de subtotal
    NoPedido: 1234          // Ejemplo de número de pedido
};

// Función para guardar el detalle del pedido
async function guardarDetallePedido() {
    try {
        const response = await fetch('https://localhost:7120/api/DetallePedido/Guardar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',  // Asegúrate de que el contenido sea JSON
            },
            body: JSON.stringify(datosDetalle)  // Convertir los datos a formato JSON
        });

        if (!response.ok) {
            throw new Error('Error al guardar el detalle del pedido');
        }

        const resultado = await response.json();
        console.log('Resultado:', resultado); // Muestra la respuesta de la API
        alert('Detalle de pedido guardado con éxito');
    } catch (error) {
        console.error('Error:', error);
        alert('Error al guardar el detalle del pedido');
    }
}

// Llamar la función para guardar el detalle
guardarDetallePedido();

// Datos del detalle de pedido a editar
const datosEditar = {
    IDDetallePedido: 1,  // ID del detalle que se va a editar
    CantidadProductos: 3, // Nueva cantidad de productos
    IDProducto: 2,       // Nuevo ID de producto
    SubtotalPedido: 150.00,  // Nuevo subtotal
    NoPedido: 1234          // Número de pedido
};

// Función para editar un detalle de pedido
async function editarDetallePedido() {
    try {
        const response = await fetch('https://localhost:7120/api/DetallePedido/Editar', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',  // Asegúrate de que el contenido sea JSON
            },
            body: JSON.stringify(datosEditar)  // Convertir los datos a formato JSON
        });

        if (!response.ok) {
            throw new Error('Error al editar el detalle del pedido');
        }

        const resultado = await response.json();
        console.log('Resultado de edición:', resultado); // Muestra la respuesta de la API
        alert('Detalle de pedido editado con éxito');
    } catch (error) {
        console.error('Error:', error);
        alert('Error al editar el detalle del pedido');
    }
}

// Llamar la función para editar el detalle
editarDetallePedido();
