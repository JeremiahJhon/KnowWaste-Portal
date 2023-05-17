var filtermapwaste = "0";
var filtermap = "Generated";
var filter1 = "1";
var filter2 = "Generated";
var filter3 = "1";
var countryid = "1";
var regionid = "1";
var subregionid = "1";
var year = "";

var activedata;

google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(GetData);

function GetData() {
    regionid = $("#country-filter select.region option:selected").val();
    subregionid = $("#country-filter select.subregion option:selected").val();
    countryid = $("#country-filter select.country option:selected").val();
    year = $("#country-filter select.year option:selected").val();

    var url = "../Data/Data?regionid=" + regionid + "&subregionid=" + subregionid + "&countryid=" + countryid + "&year=" + year;

    APICall(APICallType.GET, url, DrawChart);

    //Region
    $("#country-filter select.region").change(function () {
        var pathname = "/Data"; //window.location.pathname;
        if (pathname === "/")
            pathname = "/Home";

        regionid = $("#country-filter select.region option:selected").val();
        subregionid = $("#country-filter select.subregion option:selected").val();
        countryid = $("#country-filter select.country option:selected").val();
        year = $("#country-filter select.year option:selected").val();

        window.location.href = ".." + pathname + "?regionid=" + regionid + "&subregionid=" + subregionid + "&countryid=" + countryid + "&year=" + year;
    });

    //Subregion
    $("#country-filter select.subregion").change(function () {
        var pathname = "/Data"; //window.location.pathname;
        if (pathname === "/")
            pathname = "/Home";

        regionid = $("#country-filter select.region option:selected").val();
        subregionid = $("#country-filter select.subregion option:selected").val();
        countryid = $("#country-filter select.country option:selected").val();
        year = $("#country-filter select.year option:selected").val();

        window.location.href = ".." + pathname + "?regionid=" + regionid + "&subregionid=" + subregionid + "&countryid=" + countryid + "&year=" + year;
    });

    $("#country-filter select.country").change(function () {
        var pathname = "/Data"; //window.location.pathname;
        if (pathname === "/")
            pathname = "/Home";

        regionid = $("#country-filter select.region option:selected").val();
        subregionid = $("#country-filter select.subregion option:selected").val();
        countryid = $("#country-filter select.country option:selected").val();
        year = $("#country-filter select.year option:selected").val();

        window.location.href = ".." + pathname + "?regionid=" + regionid + "&subregionid=" + subregionid + "&countryid=" + countryid + "&year=" + year;
    });

    $("#country-filter select.year").change(function () {
        var pathname = "/Data"; //window.location.pathname;
        if (pathname === "/")
            pathname = "/Home";

        regionid = $("#country-filter select.region option:selected").val();
        subregionid = $("#country-filter select.subregion option:selected").val();
        countryid = $("#country-filter select.country option:selected").val();
        year = $("#country-filter select.year option:selected").val();

        window.location.href = ".." + pathname + "?regionid=" + regionid + "&subregionid=" + subregionid + "&countryid=" + countryid + "&year=" + year;
    });
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function DrawChart(data) {
    activedata = data;

    $(".progressicon").remove();
    $("#piechartcontainer").removeClass("hidden");
    $("#piechartcontainer2").removeClass("hidden");
    $("#combochartcontainer").removeClass("hidden");

    if (countryid === "0") {
        $("#mapcontainer").removeClass("hidden");
        DrawRegionsMap(data);
    }

    DrawFirstPieChart(data);
    DrawSecondPieChart(data);
    
    if (year === "0") { // && countryid !== "0") {
        $("#combochartcontainer").addClass("hidden");
        
        $("#combochartallyearcontainer").removeClass("hidden");
        DrawBarChartAllYear(data);
    }
    else {
        $("#combochartcontainer").removeClass("hidden");
        DrawBarChart(data);
    }

    $("#piechartcontainer .chart-filter").click(function (e) {
        e.preventDefault();

        var selected = $("#piechartcontainer .chart-filter option:selected").val();

        if (filter1 !== selected) {
            filter1 = selected;
            DrawFirstPieChart(activedata);
        }
    });

    $("#piechartcontainer2 .chart-filter").click(function (e) {
        e.preventDefault();

        var selected = $("#piechartcontainer2 .chart-filter option:selected").val();

        if (filter2 !== selected) {
            filter2 = selected;
            DrawSecondPieChart(activedata);
        }
    });

    $("#combochartcontainer .chart-filter").click(function (e) {
        e.preventDefault();

        var selected = $("#combochartcontainer .chart-filter option:selected").val();

        if (filter3 !== selected) {
            filter3 = selected;
            DrawBarChart(activedata);
        }
    });

    $("#combochartallyearcontainer .chart-filter").click(function (e) {
        e.preventDefault();

        var selected = $("#combochartallyearcontainer .chart-filter option:selected").val();

        if (filter3 !== selected) {
            filter3 = selected;
            DrawBarChartAllYear(activedata);
        }
    });

    $("#mapcontainer .filter-waste").click(function (e) {
        e.preventDefault();

        var selected = $("#mapcontainer .filter-waste option:selected").val();

        if (filtermapwaste !== selected) {
            filtermapwaste = selected;
            DrawRegionsMap(activedata);
        }
    });

    $("#mapcontainer .filter-type").click(function (e) {
        e.preventDefault();

        var selected = $("#mapcontainer .filter-type option:selected").val();

        if (filtermap !== selected) {
            filtermap = selected;
            DrawRegionsMap(activedata);
        }
    });

    $(".expand-mobile .fa").click(function (e) {
        e.preventDefault();

        if (this.className === "fa fa-chevron-down") {
            $(this).parent().css({ height: 'inherit', padding: 0, border: 'none' });
            $(this).removeClass("fa-chevron-down");
            $(this).addClass("fa-chevron-up");
        }
        else {
            $(this).parent().removeAttr('style');
            $(this).removeClass("fa-chevron-up");
            $(this).addClass("fa-chevron-down");
        }
    });
}

function DrawFirstPieChart(data) {
    //First Pie Chart

    if (countryid === "0") {
        var cat = "";

        var all = {};
        all.Generated = 0;
        all.Hazardous = 0;
        all.Collected = 0;
        all.Recycled = 0;
        all.Recovered = 0;
        all.Disposal = 0;
        all.Treatment = 0;
        all.Reuse = 0;
        all.Sludge = 0;

        for (var i = 0; i < data.length; i++) {
            if (data[i].ID === filter1) {
                cat = data[i].Category;

                all.Generated += data[i].Generated;
                all.Hazardous += data[i].Hazardous;
                all.Collected += data[i].Collected;
                all.Recycled += data[i].Recycled;
                all.Recovered += data[i].Recovered;
                all.Disposal += data[i].Disposal;
                all.Treatment += data[i].Treatment;
                all.Reuse += data[i].Reuse;
                all.Sludge += data[i].Sludge;
            }
        }

        var ar = [];
        ar.push(['Data', 'Thousand T/year']);
        if (data[i].Generated > 0)
            ar.push(['Generated', data[i].Generated])
        if (data[i].Hazardous > 0)
            ar.push(['Hazardous', data[i].Hazardous])
        if (data[i].Collected > 0)
            ar.push(['Collected', data[i].Collected])
        if (data[i].Recycled > 0)
            ar.push(['Recycled', data[i].Recycled])
        if (data[i].Recovered > 0)
            ar.push(['Recovered', data[i].Recovered])
        if (data[i].Disposal > 0)
            ar.push(['Disposal', data[i].Disposal])
        if (data[i].Treatment > 0)
            ar.push(['Treatment', data[i].Treatment])
        if (data[i].Reuse > 0)
            ar.push(['Reuse', data[i].Reuse])
        if (data[i].Sludge > 0)
            ar.push(['Sludge', data[i].Sludge])

        var pieoptions = {
            title: cat,
            vAxis: { title: 'Thousand T/year' },
            hAxis: { title: filter2 + ' Type' },
            seriesType: 'bars',
            chartArea: { width: "50%", height: "70%" }
        };

        var piechart = new google.visualization.ComboChart(document.getElementById('piechart'));
        piechart.draw(pie, pieoptions);

    }
    else {
        var handle = true;

        for (var i = 0; i < data.length; i++) {
            if (data[i].ID === filter1) {

                var ar = [];
                var ledger = [];
                var dta = [];
                ledger.push('Data')
                if (data[i].Generated > 0)
                    ledger.push('Generated');
                if (data[i].Hazardous > 0)
                    ledger.push('Hazardous');
                if (data[i].Collected > 0)
                    ledger.push('Collected');
                if (data[i].Recycled > 0)
                    ledger.push('Recycled');
                if (data[i].Recovered > 0)
                    ledger.push('Recovered');
                if (data[i].Disposal > 0)
                    ledger.push('Disposal');
                if (data[i].Treatment > 0)
                    ledger.push('Treatment');
                if (data[i].Reuse > 0)
                    ledger.push('Reuse');
                if (data[i].Sludge > 0)
                    ledger.push('Sludge');

                ar.push(ledger);

                dta.push('');
                if (data[i].Generated > 0)
                    dta.push(data[i].Generated);
                if (data[i].Hazardous > 0)
                    dta.push(data[i].Hazardous);
                if (data[i].Collected > 0)
                    dta.push(data[i].Collected);
                if (data[i].Recycled > 0)
                    dta.push(data[i].Recycled);
                if (data[i].Recovered > 0)
                    dta.push(data[i].Recovered);
                if (data[i].Disposal > 0)
                    dta.push(data[i].Disposal);
                if (data[i].Treatment > 0)
                    dta.push(data[i].Treatment);
                if (data[i].Reuse > 0)
                    dta.push(data[i].Reuse);
                if (data[i].Sludge > 0)
                    dta.push(data[i].Sludge);
                ar.push(dta);

                //ar.push(['Thousand T/year', 'Data']);
                //if (data[i].Generated > 0)
                //    ar.push(['Generated', data[i].Generated])
                //if (data[i].Hazardous > 0)
                //    ar.push(['Hazardous', data[i].Hazardous])
                //if (data[i].Collected > 0)
                //    ar.push(['Collected', data[i].Collected])
                //if (data[i].Recycled > 0)
                //    ar.push(['Recycled', data[i].Recycled])
                //if (data[i].Recovered > 0)
                //    ar.push(['Recovered', data[i].Recovered])
                //if (data[i].Disposal > 0)
                //    ar.push(['Disposal', data[i].Disposal])
                //if (data[i].Treatment > 0)
                //    ar.push(['Treatment', data[i].Treatment])
                //if (data[i].Reuse > 0)
                //    ar.push(['Reuse', data[i].Reuse])
                //if (data[i].Sludge > 0)
                //    ar.push(['Sludge', data[i].Sludge])

                if (ledger.length <= 1) {
                    handle = true;
                    break;
                }
                var pie = google.visualization.arrayToDataTable(ar);

                var pieoptions = {
                    title: data[i].Category,
                    vAxis: { title: 'Thousand T/year' },
                    hAxis: { title: data[i].Category },
                    seriesType: 'bars',
                    chartArea: { width: "50%", height: "70%" }
                };

                var piechart = new google.visualization.ComboChart(document.getElementById('piechart'));
                piechart.draw(pie, pieoptions);

                
                handle = false;
                break;
            }
        }

        if (handle) {
            $("#piechart").html("<div class='no-data'>No data available.</div>");
        }
    }
}

function DrawSecondPieChart(data) {
    //Second Pie Chart
    var arraypie2 = [];
    arraypie2.push(['Data', 'Thousand T/year']);

    if (countryid === "0") {
        var all = [];
        var cat = [];

        //Add all categories
        for (var i = 0; i < data.length; i++) {
            if (data[i].Type === 1 && all[data[i].Category] === undefined) {
                all[data[i].Category] = 0;
                cat.push(data[i].Category);
            }
        }

        for (var i = 0; i < data.length; i++) {
            if (data[i].Type === 1) {

                switch (filter2) {
                    case "Generated":
                        all[data[i].Category] += data[i].Generated;
                        break;

                    case "Hazardous":
                        all[data[i].Category] += data[i].Hazardous;
                        break;

                    case "Collected":
                        all[data[i].Category] += data[i].Collected;
                        break;

                    case "Recycled":
                        all[data[i].Category] += data[i].Recycled;
                        break;

                    case "Recovered":
                        all[data[i].Category] += data[i].Recovered;
                        break;

                    case "Disposal":
                        all[data[i].Category] += data[i].Disposal;
                        break;

                    case "Treatment":
                        all[data[i].Category] += data[i].Treatment;
                        break;

                    case "Reuse":
                        all[data[i].Category] += data[i].Reuse;
                        break;

                    case "Sludge":
                        all[data[i].Category] += data[i].Sludge;
                        break;
                }
            }
        }

        for (var i = 0; i < cat.length; i++) {
            var item = [cat[i], all[cat[i]]];
            arraypie2.push(item);
        }
    }
    else {
        if (year === "0") {
            var summary = [];
            var categories = [];

            for (var i = 0; i < data.length; i++) {
                if (categories.indexOf(data[i].Category) === -1) {
                    categories.push(data[i].Category);
                }
            }


            var index = 0;

            for (var i = 0; i < data.length; i++) {
                index = categories.indexOf(data[i].Category);

                if (summary[index] === undefined)
                    summary[index] = JSON.parse(JSON.stringify(data[i]));
                else {
                    summary[index].Generated += data[i].Generated;
                    summary[index].Hazardous += data[i].Hazardous;
                    summary[index].Collected += data[i].Collected;
                    summary[index].Recycled += data[i].Recycled;
                    summary[index].Recovered += data[i].Recovered;
                    summary[index].Disposal += data[i].Disposal;
                    summary[index].Treatment += data[i].Treatment;
                    summary[index].Reuse += data[i].Reuse;
                    summary[index].Sludge += data[i].Sludge;
                }
            }

            data = summary;
        }

        var legend = [];
        var dta = [];
        legend.push('Data');
        dta.push('');

        for (var i = 0; i < data.length; i++) {
            if (data[i].Type === 1) {

                switch (filter2) {
                    case "Generated":
                        if (data[i].Generated > 0) {
                            var item = [data[i].Category, data[i].Generated];
                            arraypie2.push(item);
                            legend.push(data[i].Category);
                            dta.push(data[i].Generated);
                        }
                        break;

                    case "Hazardous":
                        if (data[i].Collected > 0) {
                            var item = [data[i].Category, data[i].Hazardous];
                            arraypie2.push(item);
                            legend.push(data[i].Category);
                            dta.push(data[i].Generated);
                        }
                        break;

                    case "Collected":
                        var item = [data[i].Category, data[i].Collected];
                        arraypie2.push(item);
                        legend.push(data[i].Category);
                        dta.push(data[i].Collected);
                        break;

                    case "Recycled":
                        if (data[i].Recycled > 0) {
                            var item = [data[i].Category, data[i].Recycled];
                            arraypie2.push(item);
                            legend.push(data[i].Category);
                            dta.push(data[i].Recycled);
                        }
                        break;

                    case "Recovered":
                        if (data[i].Recovered > 0) {
                            var item = [data[i].Category, data[i].Recovered];
                            arraypie2.push(item);
                            legend.push(data[i].Category);
                            dta.push(data[i].Recovered);
                        }
                        break;

                    case "Disposal":
                        if (data[i].Disposal > 0) {
                            var item = [data[i].Category, data[i].Disposal];
                            arraypie2.push(item);
                            legend.push(data[i].Category);
                            dta.push(data[i].Disposal);
                        }
                        break;

                    case "Treatment":
                        if (data[i].Treatment > 0) {
                            var item = [data[i].Category, data[i].Treatment];
                            arraypie2.push(item);
                            legend.push(data[i].Category);
                            dta.push(data[i].Treatment);
                        }
                        break;

                    case "Reuse":
                        if (data[i].Reuse > 0) {
                            var item = [data[i].Category, data[i].Reuse];
                            arraypie2.push(item);
                            legend.push(data[i].Category);
                            dta.push(data[i].Reuse);
                        }
                        break;

                    case "Sludge":
                        if (data[i].Sludge > 0) {
                            var item = [data[i].Category, data[i].Sludge];
                            arraypie2.push(item);
                            legend.push(data[i].Category);
                            dta.push(data[i].Sludge);
                        }
                        break;
                }
            }
        }
    }

    var handle = true;
    
    for (var i = 1; i < dta.length; i++) {
        if (dta[i] !== 0) {
            handle = false;
            break;
        }
    }
    arraypie2 = [];
    arraypie2.push(legend);
    arraypie2.push(dta);
    if (!handle) {
        var pie2 = google.visualization.arrayToDataTable(arraypie2);

        var pieoptions2 = {
            title: filter2,
            vAxis: { title: 'Thousand T/year' },
            hAxis: { title: filter2 + ' Type' },
            seriesType: 'bars',
            chartArea: { width: "50%", height: "70%" }
        };

        var piechart2 = new google.visualization.ComboChart(document.getElementById('piechart2'));
        piechart2.draw(pie2, pieoptions2);
    }
    else {
        $("#piechart2").html("<div class='no-data'>No data available.</div>");
    }
}

function DrawBarChart(data) {
    //Bar Chart
    var array = [];
    array.push(['Data', 'Generated', 'Hazardous', 'Collected', 'Recycled', 'Recovered', 'Disposal', 'Treatment', 'Reuse', 'Sludge']);


    if (countryid === "0") {
        var all = [];
        var cat = [];

        //Add all categories
        for (var i = 0; i < data.length; i++) {
            if (data[i].Type.toString() === filter3 && all[data[i].Category] === undefined) {
                all[data[i].Category] = [0, 0, 0, 0, 0, 0, 0, 0, 0];
                cat.push(data[i].Category);
            }
        }

        for (var i = 0; i < data.length; i++) {
            if (data[i].Type.toString() === filter3) {
                all[data[i].Category][0] += data[i].Generated;
                all[data[i].Category][1] += data[i].Hazardous;
                all[data[i].Category][2] += data[i].Collected;
                all[data[i].Category][3] += data[i].Recycled;
                all[data[i].Category][4] += data[i].Recovered;
                all[data[i].Category][5] += data[i].Disposal;
                all[data[i].Category][6] += data[i].Treatment;
                all[data[i].Category][7] += data[i].Reuse;
                all[data[i].Category][8] += data[i].Sludge;
            }
        }

        var output;

        for (var i = 0; i < cat.length; i++) {
            output = all[cat[i]];
            var item = [cat[i], output[0], output[1], output[2], output[3], output[4], output[5], output[6], output[7], output[8]];
            array.push(item);
        }
    }
    else {
        if (year === "0") {
            var summary = [];
            var categories = [];

            for (var i = 0; i < data.length; i++) {
                if (categories.indexOf(data[i].Category) === -1) {
                    categories.push(data[i].Category);
                }
            }


            var index = 0;

            for (var i = 0; i < data.length; i++) {
                index = categories.indexOf(data[i].Category);
                if (summary[index] === undefined)
                    summary[index] = JSON.parse(JSON.stringify(data[i]));
                else {
                    summary[index].Generated += data[i].Generated;
                    summary[index].Hazardous += data[i].Hazardous;
                    summary[index].Collected += data[i].Collected;
                    summary[index].Recycled += data[i].Recycled;
                    summary[index].Recovered += data[i].Recovered;
                    summary[index].Disposal += data[i].Disposal;
                    summary[index].Treatment += data[i].Treatment;
                    summary[index].Reuse += data[i].Reuse;
                    summary[index].Sludge += data[i].Sludge;
                }
            }

            data = summary;
        }

        for (var i = 0; i < data.length; i++) {
            if (data[i].Type.toString() === filter3) {
                var item = [data[i].Category, data[i].Generated, data[i].Hazardous, data[i].Collected, data[i].Recycled, data[i].Recovered, data[i].Disposal, data[i].Treatment, data[i].Reuse, data[i].Sludge];
                array.push(item);
            }
        }
    }

    if (array.length > 1) {
        var combo = google.visualization.arrayToDataTable(array);

        var title = "";

        switch (filter3) {
            case "1":
                title = "Solid Waste";
                break;

            case "2":
                title = "Waste Water";
                break;

            case "3":
                title = "Gaseous Emissions";
                break;
        }

        var combooptions = {
            title: title,
            vAxis: { title: 'Thousand T/year' },
            hAxis: { title: 'Waste Type' },
            seriesType: 'bars'
        };

        var combochart = new google.visualization.ComboChart(document.getElementById('combochart'));
        combochart.draw(combo, combooptions);
    }
    else {
        $("#combochart").html("<div class='no-data'>No data available.</div>");
    }
}

function DrawBarChartAllYear(data) {
    //Bar Chart
    var years = [];

    for (var i = 0; i < data.length; i++) {
        if (years.indexOf(data[i].Year) === -1) {
            years.push(data[i].Year);
        }
    }

    years.sort();

    var array = [];
    var temparray = [];
    temparray.push("Data");

    var categories = [];

    var initarray;

    if (year === "0") {
        var summary = [];
        var index = 0;
        var yindex;

        for (var i = 0; i < data.length; i++) {
            if (data[i].Type.toString() === filter3 && temparray.indexOf(data[i].Category) === -1) {
                categories.push([data[i].Category, 0]);
                temparray.push(data[i].Category);
            }
        }

        array.push(temparray);

        for (var i = 0; i < years.length; i++) {
            for (var j = 0; j < data.length; j++) {
                if (data[j].Year === years[i] && data[j].Type.toString() === filter3) {
                    yindex = years.indexOf(years[i]);
                    //summary[yindex]

                    if (summary[yindex] === undefined) {
                        initarray = [];

                        initarray.push(years[i]);

                        for (var k = 0; k < categories.length; k++) {
                            initarray.push(0);
                        }

                        summary[yindex] = initarray;
                    }

                    index = temparray.indexOf(data[j].Category);

                    //if (yindex === 11 && index === 5)
                    //    alert();

                    summary[yindex][index] += data[j].Generated +
                        data[j].Hazardous +
                        data[j].Collected +
                        data[j].Recycled +
                        data[j].Recovered +
                        data[j].Disposal +
                        data[j].Treatment +
                        data[j].Reuse +
                        data[j].Sludge;
                }
            }
        }
    }

    summary.forEach(function (element) {
        var item = [];

        item.push(element[0].toString());

        for (var i = 1; i < element.length; i++) {
            item.push(element[i]);
        }

        array.push(item);
    });

    if (array.length > 1) {
        var combo = google.visualization.arrayToDataTable(array);

        var title = "";

        switch (filter3) {
            case "1":
                title = "Solid Waste";
                break;

            case "2":
                title = "Waste Water";
                break;

            case "3":
                title = "Gaseous Emissions";
                break;
        }

        var combooptions = {
            title: title,
            vAxis: { title: 'Thousand T/year' },
            hAxis: { title: 'Year' },
            seriesType: 'bars'
        };

        var combochart = new google.visualization.ComboChart(document.getElementById('combochartallyear'));
        combochart.draw(combo, combooptions);
    }
    else {
        $("#combochartallyear").html("<div class='no-data'>No data available.</div>");
    }
}


function DrawRegionsMap(data) {
    var all = [];
    var country = [];

    var items = [];
    items.push(['Country', filtermap]);

    for (var i = 0; i < data.length; i++) {
        if (all[data[i].Country] === undefined) {
            all[data[i].Country] = 0;
            country.push(data[i].Country);
        }
    }

    for (var i = 0; i < data.length; i++) {
        if (filtermapwaste !== "0" && data[i].ID !== filtermapwaste)
            continue;

        switch (filtermap) {
            case "Generated":
                all[data[i].Country] += data[i].Generated;
                break;

            case "Hazardous":
                all[data[i].Country] += data[i].Hazardous;
                break;

            case "Collected":
                all[data[i].Country] += data[i].Collected;
                break;

            case "Recycled":
                all[data[i].Country] += data[i].Recycled;
                break;

            case "Recovered":
                all[data[i].Country] += data[i].Recovered;
                break;

            case "Disposal":
                all[data[i].Country] += data[i].Disposal;
                break;

            case "Treatment":
                all[data[i].Country] += data[i].Treatment;
                break;

            case "Reuse":
                all[data[i].Country] += data[i].Reuse;
                break;

            case "Sludge":
                all[data[i].Country] += data[i].Sludge;
                break;
        }
    }

    for (var i = 0; i < country.length; i++) {
        var item = [country[i], all[country[i]]];
        items.push(item);
    }

    var data = google.visualization.arrayToDataTable(items);

    var options = {
        title: filtermap,
        region: '142',
        colorAxis: { colors: ['#F44336', '#E91E63', '#9C27B0', '#673AB7', '#009688', '#8BC34A', '#FF9800'] },
    };

    var chart = new google.visualization.GeoChart(document.getElementById('mapchart'));
    chart.draw(data, options);
}

APICallType = {
    GET: { value: 0 },
    POST: { value: 1 }
};

function APICall(type, url, func, data) {
    switch (type) {
        case APICallType.GET:

            $.ajax({
                type: 'GET',
                url: url,
                contentType: 'application/json; charset=utf-8',
                async: true,
                cache: false,
                success: function (data) {
                    func(data);
                },
                error: function (data) {
                    func(data);
                }
            });

            break;

        case APICallType.POST:
            $.support.cors = true;
            $.ajax({
                type: 'POST',
                crossDomain: true,
                dataType: 'json',
                data: data,
                url: url,
                async: true,
                cache: false,
                success: function (data) {
                    func(data);
                },
                error: function (data) {
                    func(data);
                }
            });

            break;
    }
};