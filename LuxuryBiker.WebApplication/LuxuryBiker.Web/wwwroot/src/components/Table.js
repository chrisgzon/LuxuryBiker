import DataTable from "react-data-table-component";

export default function({Columns, Data, Title = ""}) {

    const paginacionOpciones = {
        rowsPerPageText: "Filas por pagina",
        rangoSeparadorText: "de"
    }

    return(
        <DataTable 
            columns={Columns}
            data={Data}
            title={Title}
            pagination
            paginationComponentOptions={paginacionOpciones}
            fixedHeader
            fixedHeaderScrollHeight="600px"
            noDataComponent="No hay datos para mostrar"
        />
    );
}