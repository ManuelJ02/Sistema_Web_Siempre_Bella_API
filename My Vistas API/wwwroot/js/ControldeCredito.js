function ListarDetallesControlCredito() {
    fetch('https://localhost:7120/api/DetalleControlCredito/Listar')
        .then(response => response.json())
        .then(data => {
            const detalletabla = document.getElementById('detalleControlCreditoTabla').getElementsByTagName('tbody')[0];
            detalletabla.innerHTML = "";

            data.response.forEach(detalle => {
                let row = detalletabla.insertRow();
                row.innerHTML = `
                    <td>${detalle.IDDetalleCredito}</td>
                    <td>${detalle.MontoCuota}</td>
                    <td>${detalle.NumeroCuotas}</td>
                    <td>${detalle.IDTipoFactura}</td>
                    <td>${detalle.NoFactura}</td>
                `;
            });
        })

        .catch(error => console.error('Error al obtener los detalles de Creditos:', error));
}

function InsertarDetalleControlCredito() {
    document.getElementById('detalleControlCreditoForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const montoCuota = document.getElementById('montoCuota').value;
        const numeroCuotas = document.getElementById('numeroCuotas').value;
        const idTipoFactura = document.getElementById('idTipoFactura').value;
        const noFactura = document.getElementById('noFactura').value;

        const detalleControlCredito = {
            MontoCuota: parseFloat(montoCuota),
            NumeroCuotas: parseInt(numeroCuotas),
            IDTipoFactura: parseInt(idTipoFactura),
            NoFactura: parseInt(noFactura)
        };

        fetch('https://localhost:7120/api/DetalleControlCredito/Guardar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(detalleControlCredito)
        })
            .then(response => response.json())
            .then(data => {
                console.log('Detalle Credito Guardado:', data);
                ListarDetallesControlCredito();
            })
            .catch(error => console.error('Error al guardar los detalles de Credito:', error));

    })
}

function ObtenerDetalleControlCredito(idDetalleControlCredito) {
    fetch('https://localhost:7120/api/DetalleControlCredito/Obtener/${idDetalleCreditoCredito}')
        .then(response => response.json())
        .then(data => {
            console.log('detalle Control de Credito obtenido:', data);
        })
        .catch(error => console.error('error al obtener el detalle del control de credito:', error));
}


function ListarControlCredito() {
    fech('https://localhost:7120/api/ControlCredito/Listar')
        .then(response => response.json())
        .then(data => {
            const creditoTabla = document.getElementById('creditoTabla').getElementsByTagName('tbody')[0];
            creditoTabla.innerHTML = '';
            data.response.forEach(controlCredito => {
                let row = creditoTabla.insertRow();
                row.innerHTML = ` 
                <td>${controlCredito.IDTipoFactura}</td>
                    <td>${controlCredito.FechaInicial}</td>
                    <td>${controlCredito.Interes}</td>
                    <td>${controlCredito.Plazos}</td>
                     
                `;
            });
        })
        .catch(error => console.error('error al obtener los creditos:', error));
}

function GuardarControlCredito() {
    document.getElementById('controlCreditoForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const fechaInicial = document.getElementById('fechaInicial').value;
        const interes = document.getElementById('interes').value;
        const plazos = document.getElementById('Plazos').value;

        const controCredito = {
            fechaInicial: Dateparse(fechaInicial),
            interes: parseInt(interes),
            plazos: parseFloat(plazos)
        };

        fetch('https://localhost:7120/api/ControlCredito/Guardar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(controlCredito)
        })
            .then(response => response.json())
            .then(data => {
                console.log('Credito guardado:', data);
                ListarControlCredito();
            })
            .catch(error => console.error('error al guardar el credito:', error));

    })

}

function ObtenerControlCredito(idControlCredito) {
    fetch('https://localhost:7120/api/ControlCredito/Obtener/${idControlCredito}')
        .then(response => response.json())
        .then(data => {
            console.log('Credito obtenido:', data);
        })
        .catch(error => console.error('error al obtener el Credito:', error));
}

