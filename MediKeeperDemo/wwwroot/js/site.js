// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var itemArray = [1, 2, 3, 4];
function populateItems() {    
    $.ajax({
        method: "POST",
        url: "/Controllers/HomeController/Init",
        dataType: "json",
        timeout: 100000,
        success: function (data) {
            console.info(data.message);
            var items = data.items;
            for (var i = 0; i < items.length; i++) {
                $("#itemContainer tr:last").after('<tr><td></td><td>' + items[i].id + '</td><td>' + items[i].name + '</td><td>' + items[i].cost + '</td></tr>');
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