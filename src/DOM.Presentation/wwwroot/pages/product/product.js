$(document).ready(function () {
    Get();
});

function Form() {
    let html = "";

    html += "<div>";
    html += "<label>Name</label>";
    html += "<input type='text' id='name' class='form-control'>";
    html += "</div>";

    html += "<div>";
    html += "<label>Decription</label>";
    html += "<input type='text' id='description' class='form-control'>";
    html += "</div>";

    html += "<div>";
    html += "<label>Url Image</label>";
    html += "<input type='text' id='urlImage' class='form-control'>";
    html += "</div>";

    html += "<div>";
    html += "<label>Price</label>";
    html += "<input type='text' id='price' class='form-control'>";
    html += "</div>";

    return html;
}

function Register() {
    $(".modal-title").html("Register");

    let html = "";

    html += Form();
    html += "<hr>";
    html += "<a id='btnRegister'>Register</a>";

    $(".modal-body").html(html);

    $("#btnRegister").click(function () {
        let Obj = new Object();

        Obj.Name = $("#name").val();
        Obj.Description = $("#description").val();
        Obj.UrlImage = $("#urlImage").val();
        Obj.Price = $("#price").val();

        JAjaxSync("POST", "/v1/api/product", JSON.stringify(Obj), null, null, null);

        iziToast.success({
            title: 'OK',
            message: 'Successfully inserted record!',
        });

        Get();

        $(".modal-title").html("");
        $(".modal-body").html("");
        $(".btn-close").click();
    });
}

function Edit(uid) {
    $(".modal-title").html("Edit");

    let html = "";
    html += "<input type='hidden' id='uid' value='" + uid + "' />";
    html += Form();
    html += "<hr>";
    html += "<a id='btnEdit'>Edit</a>";

    $(".modal-body").html(html);

    let result = JAjaxReturnLineSync("GET", "/v1/api/product/" + uid, null, null, null, null);

    $("#name").val(result.name);
    $("#description").val(result.description);
    $("#urlImage").val(result.urlImage);
    $("#price").val(result.price);

    $("#btnEdit").click(function () {
        let Obj = new Object();

        Obj.Name = $("#name").val();
        Obj.Description = $("#description").val();
        Obj.UrlImage = $("#urlImage").val();
        Obj.Price = $("#price").val();


        JAjaxSync("PUT", "/v1/api/product/" + $("#uid").val(), JSON.stringify(Obj), null, null, null);
        iziToast.success({
            title: 'OK',
            message: 'Successfully updated record',
        });
        Get();

        $(".modal-title").html("");
        $(".modal-body").html("");
        $(".btn-close").click();
    });
}

function Get() {
    let html = "";

    let result = JAjaxReturnLineSync("GET", "/v1/api/product", null, null, null, null);

    html += "<a data-bs-toggle=\"modal\" data-bs-target=\"#modal\" onclick='Register()'>Register</a>";
    html += "<hr />";

    html += "<table class='table'>";
    html += "<thead>";
    html += "<tr>";
    html += "<th>Name</th>";
    html += "<th>Description</th>";
    html += "<th>Image</th>";
    html += "<th>Action</th>";
    html += "<th>Price</th>";
    html += "</tr>";
    html += "</thead>";

    html += "<tbody>";
    if (result.length > 0) {
        $.each(result, function (index, json) {
            html += "<tr>";
            html += "<td>" + json.name + "</td>";
            html += "<td>" + json.description + "</td>";
            html += "<td> <img style='height:50px' src='" + json.urlImage + "'/></td>";
            html += "<td><a  data-bs-toggle=\"modal\" data-bs-target=\"#modal\" onclick='Edit(\"" + json.uid + "\")'><i class=\"fa-solid fa-pen-to-square\"></i></a> <a onclick='Delete(\"" + json.name + "\",\"" + json.uid + "\")'><i class=\"fa-solid fa-trash\"></i></a></td>";
            html += "<td>" + json.price + "</td>";
            html += "</tr>";
        });
    } else {
        html += "<tr>";
        html += "<td colspan='100'>No records registered</td>";
        html += "</tr>";
    }
    html += "</tbody>";
    html += "</table>";

    $("#divBody").html(html);
}

function Delete(value, uid) {
    iziToast.show({
        theme: 'dark',
        icon: 'icon-person',
        title: 'Hey',
        message: 'Do you want to delete the record: ' + value + '?',
        position: 'bottomRight', // bottomRight, bottomLeft, topRight, topLeft, topCenter, bottomCenter
        progressBarColor: 'rgb(0, 255, 184)',
        buttons: [
            ['Ok', function (instance, toast) {
                JAjaxSync("DELETE", "/v1/api/product/" + uid, null, null, null, null);
                iziToast.success({
                    title: 'OK',
                    message: 'Successfully Deleted record!',
                });
                Get();
            }, true], // true to focus
            ['Close', function (instance, toast) {
                instance.hide({
                    transitionOut: 'fadeOutUp',
                    onClosing: function (instance, toast, closedBy) {
                        console.info('closedBy: ' + closedBy); // The return will be: 'closedBy: buttonName'
                    }
                }, toast, 'buttonName');
            }]
        ],
        onOpening: function (instance, toast) {
            console.info('callback abriu!');
        },
        onClosing: function (instance, toast, closedBy) {
            console.info('closedBy: ' + closedBy); // tells if it was closed by 'drag' or 'button'
        }
    });
}