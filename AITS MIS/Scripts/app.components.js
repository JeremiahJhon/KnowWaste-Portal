var components = {};

components.table = function (d) {
    this.data = d;

    this.Generate = function () {
        var output = "<table id='datatable'>";

        for (var i = 0; i < this.data.Rows.length; i++) {
            output += "<tr>";

            for (var j = 0; j < this.data.Columns.length; j++) {
                output += "<td>" + this.data.Rows[j] + "</td>";
            }

            output += "</tr>";
        }

        output += "</table>";

        $("body").append(output);
        $("#datatable").DataTable();
    };
}