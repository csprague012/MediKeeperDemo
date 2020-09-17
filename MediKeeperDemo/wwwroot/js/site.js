$(document).ready(function () {
    var items = {};
    function populateItems() {
        $.ajax({
            method: "GET",
            url: "/Controllers/HomeController/Init",
            dataType: "json",
            timeout: 100000,
            success: function (data) {
                console.info(data.message);
                items = data.items;
                $("#itemContainer").empty();
                $("#itemContainer").append("<tr hidden></tr>");
                for (var i = 0; i < items.length; i++) {
                    $("#itemContainer tr:last").after('<tr id="row' + i + '"><td><button class="btn btn-sm btn-default" onclick="editRow(' + i + ')"><i class="fa fa-pencil"></i></button</td><td>' + items[i].id + '</td><td>' + items[i].name + '</td><td>' + items[i].cost + '</td></tr>');
                }
            },
            error: function (e) {
                console.info("Error");
            },
            done: function (e) {
                console.info("DONE");
            }
        });

    }
    populateItems();

    addRow = function () {
        $("#newRow").remove();
        $("#itemContainer tr:last").after('<tr id="newRow"><td><button class="btn btn-sm btn-info" onclick="saveRow()"><i class="fa fa-save"></i></button></td><td></td><td><input id="newRowName"></input></td><td><input id="newRowCost" type="number"></input></td></tr>');
    }
    editRow = function (index) {
        $("#row" + index).hide();
        $("#newRow").remove();
        var rowName = items[index].name;
        var rowCost = items[index].cost;
        var rowId = items[index].id;
        $("#row" + index).after('<tr id="rowEdit' + index + '"><td><button class="btn btn-sm btn-info" onclick="saveRow(' + index + ')"><i class="fa fa-save"></i></button><button class="btn btn-sm btn-danger" onclick="deleteRow(' + index + ')"><i class="fa fa-trash"></i></button></td><td>' + rowId + '</td><td><input id="newRowName" value="' + rowName + '"></input></td><td><input id="newRowCost" type="number" value="' + rowCost + '"></input></td></tr>');
    }
    saveRow = function (index) {
        var type = "";
        var id = null;
        var name = $("#newRowName").val();
        var cost = $("#newRowCost").val();

        if (index != null || index != undefined) {
            $("#rowEdit" + index).remove();
            id = items[index].id;
            type = "update";
        }
        else {
            $("#newRow").remove();
            type = "add";
        }
        var payload = { id: id, name: name, cost: cost };
        console.log(name + " - " + cost);
        switch (type) {
            case "update":
                $.ajax({
                    method: "POST",
                    url: "/Controllers/HomeController/Update",
                    dataType: "json",
                    data: payload,
                    timeout: 100000,
                    success: function (data) {
                        console.info(data);
                        populateItems();
                    },
                    error: function (e) {
                        console.info("Error");
                    },
                    done: function (e) {
                        console.info("DONE");
                    }
                });
                break;
            case "add":
                $.ajax({
                    method: "POST",
                    url: "/Controllers/HomeController/Add",
                    dataType: "json",
                    data: payload,
                    timeout: 100000,
                    success: function (data) {
                        console.info(data);
                        populateItems();
                    },
                    error: function (e) {
                        console.info("Error");
                    },
                    done: function (e) {
                        console.info("DONE");
                    }
                });
                break;
        }

    }
    deleteRow = function (index) {
        var payload = items[index];
        $.ajax({
            method: "POST",
            url: "/Controllers/HomeController/Delete",
            dataType: "json",
            data: payload,
            timeout: 100000,
            success: function (data) {
                console.info(data.message);
                populateItems();
            },
            error: function (e) {
                console.info("Error");
            },
            done: function (e) {
                console.info("DONE");
            }
        });
    }
    loadMaxVals = function () {
        $("#MaxValContainer").empty();
        var name = $("#itemName").val();
        var payload = { name: name };
        if (name.length > 0) {
            $.ajax({
                method: "POST",
                url: "/Controllers/HomeController/GetMaxPriceByName",
                dataType: "json",
                data: payload,
                timeout: 100000,
                success: function (data) {
                    console.info(data.message);
                    for (var i = 0; i < data.items.length; i++) {
                        $("#MaxValContainer").append('<tr><td>' + data.items[i].name + '</td><td>' + data.items[i].cost + '</td></tr>');
                    }
                },
                error: function (e) {
                    console.info("Error");
                },
                done: function (e) {
                    console.info("DONE");
                }
            });
        }
        else {
            $.ajax({
                method: "GET",
                url: "/Controllers/HomeController/GetMaxPrice",
                dataType: "json",
                timeout: 100000,
                success: function (data) {
                    console.info(data.message);
                    for (var i = 0; i < data.items.length; i++) {
                        $("#MaxValContainer").append('<tr><td>' + data.items[i].name + '</td><td>' + data.items[i].cost + '</td></tr>');
                    }
                },
                error: function (e) {
                    console.info("Error");
                },
                done: function (e) {
                    console.info("DONE");
                }
            });
        }
    }
    function importFile(e) {
        var file = e.target.files[0];
        var reader = new FileReader();
        reader.readAsText(file);
        reader.onload = function (event) {
            var csvData = { data: event.target.result };
            $.ajax({
                method: "POST",
                url: "/Controllers/HomeController/UploadCSV",
                dataType: "json",
                data: csvData,
                timeout: 100000,
                success: function (data) {
                    console.info(data.message);
                    window.location.href = "/";
                },
                error: function (e) {
                    console.info("Error");
                },
                done: function (e) {
                    console.info("DONE");
                }
            });
        };
    }
    document.getElementById('newFile').addEventListener('change', importFile, false);
    
});