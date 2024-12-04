// Al cargar la página, obtenemos las marcas, categorías y productos
document.addEventListener('DOMContentLoaded', () => {
    fetchMarcas();
    fetchCategorias();
    fetchProductos();
});



// Función para obtener las marcas desde la API
function fetchMarcas() {

    const marca = {
        nombre: nombreMarca
    };

    fetch('https://localhost:7120/api/Marca/Lista', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {

            const marcaSelect = document.getElementById('marca');
            marcaSelect.innerHTML = '<option value="">Seleccione una marca</option>'; // Limpiar antes de agregar

            if (Array.isArray(data)) {
                data.forEach(marca => {
                    const option = document.createElement('option');
                    option.value = marca.id;
                    option.textContent = marca.nombre;
                    marcaSelect.appendChild(option);
                });
            })
        .catch(error => {
            console.error('Error al obtener marcas:', error);
        }{


        }
}



// Función para obtener las categorías desde la API
function fetchCategorias() 
{
    fetch('https://localhost:7120/api/Categoria', 
    {
        method: 'GET',
        headers: 
        {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            const categoriaselect = document.getElementById('categoria');
            categoriaselect.innerHTML = '<option value="">Seleccione una categoría</option>'; // Limpiar antes de agregar

            data.forEach(categoria => {
                const option = document.createElement('option');
                option.value = categoria.id;
                option.textContent = categoria.nombre;
                categoriaselect.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error al obtener categorías:', error);
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
            const productosTable = document.getElementById('productosTable').getElementsByTagName('tbody')[0];
            productosTable.innerHTML = ''; // Limpiar tabla antes de agregar

            data.forEach(producto => {
                const row = productosTable.insertRow();
                row.innerHTML = `
                <td>${producto.nombre}</td>
                <td>${producto.marca}</td>
                <td>${producto.categoria}</td>
                <td>${producto.precio}</td>
                <td>
                    <button onclick="editarProducto(${producto.id})">Editar</button>
                    <button onclick="eliminarProducto(${producto.id})">Eliminar</button>
                </td>
            `;
            });
        })
        .catch(error => {
            console.error('Error al obtener productos:', error);
        });
}

// Función para registrar un nuevo producto
document.getElementById('formProducto').addEventListener('submit', function (event) {
    event.preventDefault();

    const nombreProducto = document.getElementById('nombreProducto').value;
    const marca = document.getElementById('marca').value;
    const categoria = document.getElementById('categoria').value;
    const precio = document.getElementById('precio').value;

    if (!nombreProducto || !marca || !categoria || !precio) {
        alert('Todos los campos son requeridos.');
        return;
    }

    const producto = {
        nombre: nombreProducto,
        marcaId: marca,
        categoriaId: categoria,
        precio: precio
    };

    fetch('https://localhost:7120/api/Producto', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(producto)
    })
        .then(response => response.json())
        .then(data => {
            if (data) {
                alert('Producto registrado exitosamente.');
                fetchProductos();  // Actualizar la lista de productos
            } else {
                alert('Error al registrar producto.');
            }
        })
        .catch(error => {
            console.error('Error al registrar producto:', error);
        });
});

// Función para registrar una nueva marca
document.getElementById('formMarca').addEventListener('submit', function (event) {
    event.preventDefault();

    const nombreMarca = document.getElementById('nombreMarca').value;

    if (!nombreMarca) {
        alert('El nombre de la marca es requerido.');
        return;
    }

    const marca = {
        nombre: nombreMarca
    };

    fetch('https://localhost:7120/api/Marca', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(marca)
    })
        .then(response => response.json())
        .then(data => {
            if (data) {
                alert('Marca registrada exitosamente.');
                fetchMarcas();  // Actualizar lista de marcas
            } else {
                alert('Error al registrar marca.');
            }
        })
        .catch(error => {
            console.error('Error al registrar marca:', error);
        });
});

// Función para registrar una nueva categoría
document.getElementById('formCategoria').addEventListener('submit', function (event) {
    event.preventDefault();

    const nombreCategoria = document.getElementById('nombreCategoria').value;

    if (!nombreCategoria) {
        alert('El nombre de la categoría es requerido.');
        return;
    }

    const categoria = {
        nombre: nombreCategoria
    };

    fetch('https://localhost:7120/api/Categoria', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(categoria)
    })
        .then(response => response.json())
        .then(data => {
            if (data) {
                alert('Categoría registrada exitosamente.');
                fetchCategorias();  // Actualizar lista de categorías
            } else {
                alert('Error al registrar categoría.');
            }
        })
        .catch(error => {
            console.error('Error al registrar categoría:', error);
        });
});
