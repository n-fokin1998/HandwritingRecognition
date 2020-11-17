(function () {
    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#imageHolder').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#file").change(function () {
        readURL(this);
    });

    $("#clearFileArea").click(function () {
        $("#file").val('');
        $("#imageHolder").attr('src', '/images/no-image.png');
        $('#prediction').text('?');
    });

    $('#upload-check').click(function () {
        $('#prediction').text('?');
        $.ajax({
            type: 'POST',
            url: 'upload',
            data: {
                base64Image: $("#imageHolder").attr('src')
            }
        }).done(function (msg) {
            $('#prediction').text(msg.prediction);
        });
    });
})();
