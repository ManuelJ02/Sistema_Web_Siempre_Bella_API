function ListarMarca() {
    fetch('https://localhost:7120/api/Marca/Listar')
        .then(response => response.json())

        .then(data => {
            const marcaTabla = document.getElementById('marcaTabla').getElementsByTagName('tbody')[0];
            marcaTabla.innerHTML = '';

            data.response.forEach(marca => {
                let row = marcaTabla.insertRow();
                row.innerHTML =
                    `
                    <td>${marca.IDMarca}</td>
                    <td>${marca.NombreMarca}</td>           
                    <td>
                        <button onclick= "obtenerMarca(${marca.IDMarca})">Editar</button>
                        <button onclick= "elimnarMarca(${marca.NombreMarca})">Eliminar</button>
                    </td>
                `;
            });

        })
        .catch(error => console.error('Error al obtener la lista de Marca:', error));
}



function InsertarMarca() {
    document.getElementById('marcaForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const IdMarca = document.getElementById('IdMarca').value;
        const nombreMarca = document.getElementById('nombreMarca').value;

        const marca =
        {
            NombreMarca: nombreMarca,
        };

        let url = 'https://localhost:7120/api/Marca/Guardar';
        let metodo = 'POST';

        if (IdMarca) {
            marca.IdMarca = parseInt(IdMarca);
            url = 'https://localhost:7120/api/Marca/Editar';
            metodo = 'PUT';
        }

        fetch(url,
            {
                method: metodo,
                headers:
                {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify(marca)
            })

            .then(response => response.json())
            .then(data => {
                console.log('Marca guardada:', data);
                ListarMarca();
            })

            .catch(error => console.error('Error al guardar la marca:', error));
    });

}


function BuscarMarca(id) {
    fetch('https://localhost:7120/api/Marca/Obtener/${id}')

        .then(response => response.json())
        .then(data => {
            const marca = data.response;
            documment.getElementById('nombreMarca').value = marca.NombreMarca;
            documment.getElementById('Idmarca').value = marca.IDMarca;

        })
        .catch(error => console.error('Error al obtener la Marca:', error))
}


function EliminarMarca(id) {
    if (confirm('¿Estas seguro de eliminar esta Marca?')) {
        fetch('https://localhost:7120/api/Marca/Eliminar/${id}', { method: 'DELETE', })
            .then(response => response.json())
            .then(data => {
                console.log('Marca Eliminada:', data);
                ListarMarca();
            })
            .catch(error => console.error('Error al eliminar la Marca:', error))
    }
}