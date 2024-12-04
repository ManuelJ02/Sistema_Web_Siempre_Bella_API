function ListarProducto() {
    fetch('https://localhost:7120/api/Producto/Listar')
        .then(response => response.json())

        .then(data => {
            const productoTabla = document.getElementById('productoTabla').getElementsByTagName('tbody')[0];
            productoTabla.innerHTML = '';

            data.response.forEach(producto => {
                let row = productoTabla.insertRow();
                row.innerHTML =
                    `
                    <td>${producto.IDProducto}</td>
                    <td>${producto.Stock}</td>
                    <td>${producto.Precio}</td>  
                    <td>${producto.Descripcion}</td>  
                    <td>${producto.IDMarca}</td>  
                    <td>${producto.IDCategoria}</td>  
                    <td>
                        <button onclick= "obtenerProducto(${producto.IDProducto})">Editar</button>
                        <button onclick= "elimnarProducto(${producto.NombreProducto})">Eliminar</button>
                    </td>
                `;
            });

        })
        .catch(error => console.error('Error al obtener la lista de producto:', error));
}



function InsertarProducto() {
    document.getElementById('ProductoForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const IdProducto = document.getElementById('IdProducto').value;
        const stock = document.getElementById('stock').value;
        const precio = document.getElementById('precio').value;
        const descripcion = document.getElementById('descripcion').value;
        const IdMarca = document.getElementById('IdMarca').value;
        const IdCategoria= document.getElementById('IdCategoria').value;

        const producto =
        {
            Stock: stock,
            Precio: precio,
            Descripcion: decripcion,
            IDMarca: IdMarca,
            IDCategoria:IdCategoria
        };

        let url = 'https://localhost:7120/api/Producto/Guardar';
        let metodo = 'POST';

        if (IdProducto) {
            producto.IdProducto = parseInt(IdProducto);
            url = 'https://localhost:7120/api/Producto/Editar';
            metodo = 'PUT';
        }

        fetch(url,
            {
                method: metodo,
                headers:
                {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify(producto)
            })

            .then(response => response.json())
            .then(data => {
                console.log('Producto guardado:', data);
                ListarProducto();
            })

            .catch(error => console.error('Error al guardar el producto:', error));
    });

}


function BuscarProducto(id) {
    fetch('https://localhost:7120/api/Producto/Obtener/${id}')

        .then(response => response.json())
        .then(data => {
            const producto = data.response;
            documment.getElementById('stock').value = producto.Stock;
            documment.getElementById('precio').value = producto.Precio;
            documment.getElementById('descripcion').value = producto.Descripcion;
            documment.getElementById('IdMarca').value = producto.IDMarca;
            documment.getElementById('IdCategoria').value = producto.IDCategoria;
            documment.getElementById('IdProducto').value = producto.IDProducto;

        })
        .catch(error => console.error('Error al obtener el producto:', error))
}


function EliminarProducto(id) {
    if (confirm('¿Estas seguro de eliminar este Producto?')) {
        fetch('https://localhost:7120/api/Producto/Obtener/${id}', { method: 'DELETE', })
            .then(response => response.json())
            .then(data => {
                console.log('Producto Eliminado:', data);
                ListarProducto();
            })
            .catch(error => console.error('Error al eliminar el producto:', error))
    }
}