$(document).ready(function () {
    var currentStep = 0;
    var steps = $(".form-step");
    var progressBar = $("#progressBar");
    var loadingIndicator = $("#loadingIndicator");
    var successMessage = $("#successMessage");
    var apiBaseUrl = $(".container").data("api-base-url"); 

    steps.eq(currentStep).fadeIn();
    $("#prevBtn").hide();

    $("#nextBtn").click(function () {
        if (validateStep(currentStep)) {
            if (currentStep < steps.length - 1) {
                steps.eq(currentStep).fadeOut(300, function () {
                    currentStep++;
                    steps.eq(currentStep).fadeIn(300);
                    updateProgressBar();
                    if (currentStep > 0) {
                        $("#prevBtn").show();
                    }
                    if (currentStep === steps.length - 1 && $("#profilePicture").val()) {
                        $("#nextBtn").text("Gönder");
                    }
                });
            } else if (currentStep === steps.length - 1) {
                var profilePicture = $("#profilePicture").val();
                if (profilePicture) {
                    var recaptchaResponse = grecaptcha.getResponse();
                    if (recaptchaResponse.length === 0) {
                        alert("Lütfen reCAPTCHA doğrulamasını tamamlayın.");
                    } else {
                        loadingIndicator.show();
                        submitForm();
                    }
                } else {
                    alert("Lütfen bir profil resmi yükleyin.");
                }
            }
        }
    });

    $("#prevBtn").click(function () {
        if (currentStep > 0) {
            steps.eq(currentStep).fadeOut(300, function () {
                currentStep--;
                steps.eq(currentStep).fadeIn(300);
                updateProgressBar();
                loadingIndicator.hide();
                if (currentStep === 0) {
                    $("#prevBtn").hide(); 
                }
                if (currentStep < steps.length - 1) {
                    $("#nextBtn").text("Sonraki");
                }
            });
        }
    });

    function updateProgressBar() {
        var progress = (currentStep) / (steps.length - 1) * 100;
        progressBar.css("width", progress + "%");
        progressBar.text(Math.round(progress) + "%");
    }

    function validateStep(step) {
        var valid = true;
        var inputs = steps.eq(step).find("input");
        inputs.each(function () {
            if (!this.checkValidity()) {
                this.reportValidity();
                valid = false;
                return false;
            }
        });

        if (step === 3) {
            var password = $("#password").val();
            var confirmPassword = $("#confirmPassword").val();
            if (password !== confirmPassword) {
                alert("Şifreler uyuşmuyor.");
                valid = false;
            }
        }

        return valid;
    }

    function submitForm() {
        var formData = new FormData($("#registrationForm")[0]);

        $.ajax({
            url: apiBaseUrl + "User/Add",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                loadingIndicator.hide();
                successMessage.show();
                console.log("Başarılı mesaj gösteriliyor.");
                setTimeout(function () {
                    successMessage.hide();
                    console.log("Başarılı mesaj gizleniyor.");
                    resetForm();
                }, 1500);
            },
            error: function (xhr, status, error) {
                loadingIndicator.hide();
                alert("Kayıt sırasında bir hata oluştu.");
            }
        });
    }

    function resetForm() {
        $("#registrationForm")[0].reset();
        currentStep = 0;
        steps.removeClass("active").hide();
        steps.eq(currentStep).addClass("active").fadeIn();
        $("#prevBtn").hide();
        $("#nextBtn").text("Sonraki");
        updateProgressBar();
        grecaptcha.reset(); 
    }

    $("#profilePicture").change(function () {
        if ($(this).val()) {
            $("#nextBtn").text("Gönder");
        } else {
            $("#nextBtn").text("Sonraki");
        }
    });
});
