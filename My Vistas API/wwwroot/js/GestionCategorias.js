function ListarCategoria() {
    fetch('https://localhost:7120/api/Categoria/Lista')
        .then(response => response.json())

        .then(data => {
            const categoriaTabla = document.getElementById('categoriaTabla').getElementsByTagName('tbody')[0];
            categoriaTabla.innerHTML = '';

            data.response.forEach(categoria => {
                let row = categoriaTabla.insertRow();
                row.innerHTML =
                    `
                    <td>${categoria.IDCategoria}</td>
                    <td>${empleado.NombreCategoria}</td>           
                    <td>
                        <button onclick= "obtenerCategoria(${categoria.IDCategoria})">Editar</button>
                        <button onclick= "elimnarCategoria(${categoria.NombreCategoria})">Eliminar</button>
                    </td>
                `;
            });

        })
        .catch(error => console.error('Error al obtener la lista de Categorias:', error));
}



function InsertarCategoria() {
    document.getElementById('categoriaForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const IdCategoria = document.getElementById('IdCategoria').value;
        const nombreCategoria = document.getElementById('nombreCategoria').value;

        const categoria =
        {
            NombreCategoria: nombreCategoria,
        };

        let url = 'https://localhost:7120/api/Categoria/Guardar';
        let metodo = 'POST';

        if (IdCategoria) {
            categoria.IdCategoria = parseInt(IdCategoria);
            url = 'https://localhost:7120/api/Categoria/Editar';
            metodo = 'PUT';
        }

        fetch(url,
            {
                method: metodo,
                headers:
                {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify(categoria)
            })

            .then(response => response.json())
            .then(data => {
                console.log('Categoria guardada:', data);
                ListarCategoria();
            })

            .catch(error => console.error('Error al guardar la categoria:', error));
    });

}


function BuscarCategoria(id) {
    fetch('https://localhost:7120/api/Categoria/Obtener/${id}')

        .then(response => response.json())
        .then(data => {
            const categoria = data.response;
            documment.getElementById('nombre').value = categoria.NombreCategoria;
            documment.getElementById('Idcategoria').value = categoria.IDCategoria;

        })
        .catch(error => console.error('Error al obtener la Categoria:', error))
}


function EliminarCategoria(id) {
    if (confirm('¿Estas seguro de eliminar esta Categoria?')) {
        fetch('https://localhost:7120/api/Categoria/Eliminar/${id}', { method: 'DELETE', })
            .then(response => response.json())
            .then(data => {
                console.log('Categoria Eliminada:', data);
                ListarCategoria();
            })
            .catch(error => console.error('Error al eliminar la Categoria:', error))
    }
}