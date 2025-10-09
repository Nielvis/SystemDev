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
    html += "<label>Email</label>";
    html += "<input type='text' id='email' class='form-control'>";
    html += "</div>";

    html += "<div>";
    html += "<label>Password</label>";
    html += "<input type='text' id='password' class='form-control'>";
    html += "</div>";

    html += "<div>";
    html += "<label>Gender</label>";
    html += "<input type='text' id='gender' class='form-control'>";
    html += "</div>";

    html += "<div>";
    html += "<label>BirthDate</label>";
    html += "<input type='date' id='birthDate' class='form-control'>";
    html += "</div>";


    return html;
}

function Register() {
    $(".modal-title").html("Register User");

    let html = "";

    html += Form();
    html += "<hr>";
    html += "<a id='btnRegister'>Register User</a>";

    $(".modal-body").html(html);

    $("#btnRegister").click(function () {
        let Obj = new Object();

        Obj.Name = $("#name").val();
        Obj.Email = $("#email").val();
        Obj.Password = $("#password").val();
        Obj.Gender = $("#gender").val();
        Obj.BirthDate = $("#birthDate").val();

        JAjaxSync("POST", "/v1/api/user", JSON.stringify(Obj), null, null, null);

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

    let result = JAjaxReturnLineSync("GET", "/v1/api/user/" + uid, null, null, null, null);
    console.log(result);
    $("#name").val(result.name);
    $("#email").val(result.email);
    $("#password").val(result.password); 
    $("#gender").val(result.gender);
    $("#birthDate").val(new Date(result.birthDate));

    $("#btnEdit").click(function () {
        let Obj = new Object();

        Obj.Name = $("#name").val();
        Obj.Email = $("#email").val();  
        Obj.Password = $("#password").val();
        Obj.Gender = $("#gender").val();
        Obj.BirthDate = $("#birthDate").val();  

        JAjaxSync("PUT", "/v1/api/user/" + $("#uid").val(), JSON.stringify(Obj), null, null, null);
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

    let result = JAjaxReturnLineSync("GET", "/v1/api/user", null, null, null, null);

    html += "<a data-bs-toggle=\"modal\" data-bs-target=\"#modal\" onclick='Register()'>Register User</a>";
    html += "<hr />";

    html += "<table class='table'>";
    html += "<thead>";
    html += "<tr>";
    html += "<th>Name</th>";
    html += "<th>Email</th>";
    html += "<th>Password</th>";
    html += "<th>Gender</th>";
    html += "<th>BirthDate</th>";
    html += "<th>CadastreDate</th>";
    html += "<th>Action</th>";
    html += "</tr>";
    html += "</thead>";

    html += "<tbody>";
    if (result.length > 0) {
        $.each(result, function (index, json) {
            html += "<tr>";
            html += "<td>" + json.name + "</td>";
            html += "<td>" + json.email + "</td>";
            html += "<td>" + json.password + "</td>";
            html += "<td>" + json.gender + "</td>";
            html += "<td>" + json.birthDate + "</td>";
            html += "<td>" + json.cadastreDate + "</td>";
            html += "<td><a  data-bs-toggle=\"modal\" data-bs-target=\"#modal\" onclick='Edit(\"" + json.uid + "\")'><i class=\"fa-solid fa-pen-to-square\"></i></a> <a onclick='Delete(\"" + json.name + "\",\"" + json.uid + "\")'><i class=\"fa-solid fa-trash\"></i></a></td>";
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
                JAjaxSync("DELETE", "/v1/api/user/" + uid, null, null, null, null);
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