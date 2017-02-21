$(document).ready(function () {


    $('#uploadImagesButton').click(function () {

        var uploadForm = $("#formID");

        alert(uploadForm.serialize());

        alert(uploadForm.val());

        uploadForm.Validate();

        if (!uploadForm.Valid()) {
            alert('Not valid');
            return;
        }
        var loading = $("#loadingImages");

        loading.show();

    });


    $('.imageDescriptionInGallery').hover(function () {

        var imageDescriptionInGallery = $(this).text();

        var currentImageDescriptionInGallery = $('#currentImageDescriptionInGallery');
        currentImageDescriptionInGallery.text(imageDescriptionInGallery);
        currentImageDescriptionInGallery.show();

    });

    $('.imageDescriptionInGallery').mouseleave(function () {


        var currentImageDescriptionInGallery = $('#currentImageDescriptionInGallery');
        currentImageDescriptionInGallery.hide();

    });

});
