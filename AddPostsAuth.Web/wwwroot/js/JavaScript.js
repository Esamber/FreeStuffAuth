$(() => {
    $(".form-control").on('keyup', function () {
        ensureFormValidity();
    })
    function ensureFormValidity() {
        const isValid = isFormValid();
        $("#submit").prop('disabled', !isValid);
    }
    function isFormValid() {
        const email = $("#email").val();
        const password = $("#password").val();
        return email && password;
    }
})