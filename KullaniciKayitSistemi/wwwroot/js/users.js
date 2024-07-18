$(document).ready(function () {
    var apiBaseUrl = $(".container").data("api-base-url"); 

    
    function loadUsers() {
        $.ajax({
            url: apiBaseUrl + "User/GetAll",
            type: "GET",
            beforeSend: function () {
                $("#loadingIndicator").show();
            },
            success: function (response) {
                $("#loadingIndicator").hide();
                if (response.isSuccess) {
                    populateTable(response.data);
                } else {
                    showError(response.message);
                }
            },
            error: function (xhr, status, error) {
                $("#loadingIndicator").hide();
                showError("Kullanıcıları yüklerken bir hata oluştu.");
            }
        });
    }

    function populateTable(users) {
        var tbody = $("#usersTable tbody");
        tbody.empty(); 

        users.forEach(function (user) {
            var row = "<tr data-id='" + user.id + "'>" +
                "<td>" + user.id + "</td>" +
                "<td class='name'>" + user.name + "</td>" +
                "<td class='email'>" + user.email + "</td>" +
                "<td class='password'>" + user.password + "</td>" +  
                "<td class='profilePicture'><img src='" + user.profilePicture + "' alt='Profile Picture' width='50' height='50'></td>" +
                "<td>" +
                "<button class='btn btn-primary btn-edit' data-id='" + user.id + "'>Düzenle</button> " +
                "<button class='btn btn-danger btn-delete' data-id='" + user.id + "'>Sil</button>" +
                "</td>" +
                "</tr>";
            tbody.append(row);
        });

        // Düzenle butonuna tıklama olayı
        $(".btn-edit").click(function () {
            var userId = $(this).data("id");
            var row = $("tr[data-id='" + userId + "']");
            var isEditing = $(this).text() === "Güncelle";

            if (isEditing) {
                updateUser(userId, row, $(this));
            } else {
                editUser(userId, row, $(this));
            }
        });

        // Sil butonuna tıklama olayı
        $(".btn-delete").click(function () {
            var userId = $(this).data("id");
            deleteUser(userId);
        });
    }

    // Hata mesajını göster
    function showError(message) {
        var errorMessage = $("#errorMessage");
        errorMessage.text(message);
        errorMessage.show();
    }

    function editUser(userId, row, button) {
        row.find(".name").html("<input type='text' class='form-control' value='" + row.find(".name").text() + "'>");
        row.find(".email").html("<input type='email' class='form-control' value='" + row.find(".email").text() + "'>");
        row.find(".password").html("<input type='password' class='form-control' value='" + row.find(".password").text() + "'>");
        row.find(".profilePicture").append("<input type='file' class='form-control-file' name='profilePicture'>");

        button.text("Güncelle");
    }

    function updateUser(userId, row, button) {
        var name = row.find(".name input").val();
        var email = row.find(".email input").val();
        var password = row.find(".password input").val();
        var profilePicture = row.find("input[name='profilePicture']")[0].files[0];

        var formData = new FormData();
        formData.append("Id", userId);
        formData.append("Name", name);
        formData.append("Email", email);
        formData.append("Password", password);
        if (profilePicture) {
            formData.append("ProfilePicture", profilePicture);
        }
        $("#loadingIndicator").show();
        $.ajax({
            url: apiBaseUrl + "User/Update",
            type: "PUT",
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                $("#loadingIndicator").show();
            },
            success: function (response) {
                $("#loadingIndicator").hide();
                if (response.isSuccess) {
                    row.find(".name").text(name);
                    row.find(".email").text(email);
                    row.find(".password").text(password);
                    if (profilePicture) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            row.find(".profilePicture img").attr("src", e.target.result);
                        };
                        reader.readAsDataURL(profilePicture);
                    }

                    row.find("input[name='profilePicture']").remove(); // Dosya seçme alanını kaldır
                    button.text("Düzenle");
                } else {
                    showError(response.message);
                }
            },
            error: function (xhr, status, error) {
                $("#loadingIndicator").hide();
                showError("Kullanıcıyı güncellerken bir hata oluştu.");
                setTimeout(function () {
                    $("#errorMessage").fadeOut("slow");
                }, 2000);
            }
        });
    }

    function deleteUser(userId) {
        if (confirm("Bu kullanıcıyı silmek istediğinizden emin misiniz?")) {
            $.ajax({
                url: apiBaseUrl + "User/Delete/" + userId,
                type: "DELETE",
                beforeSend: function () {
                    $("#loadingIndicator").show();
                },
                success: function (response) {
                    $("#loadingIndicator").hide();
                    if (response.isSuccess) {
                        loadUsers(); 
                    } else {
                        showError(response.message);
                    }
                },
                error: function (xhr, status, error) {
                    $("#loadingIndicator").hide();
                    showError("Kullanıcıyı silerken bir hata oluştu.");
                    setTimeout(function () {
                        $("#errorMessage").fadeOut("slow");
                    }, 2000);
                }
            });
        }
    }

    loadUsers();
});
