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


    $('.image-gallery').hover(function () {

        // Parent of image is the <a anchor>. Next is the div
        var imageDescriptionInGallery = $(this).parent().next().text();

        var currentImageDescriptionInGallery = $('#currentImageDescriptionInGallery');
        currentImageDescriptionInGallery.text(imageDescriptionInGallery);
        currentImageDescriptionInGallery.show();

    });

    $('.image-gallery').mouseleave(function () {


        var currentImageDescriptionInGallery = $('#currentImageDescriptionInGallery');
        currentImageDescriptionInGallery.hide();

    });

});
