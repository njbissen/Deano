
function validateRadio(name) {
    Validation.clearErrors('#' + name);
}

function validateOtherText(txt, chk, chkClass) {
    if (chk.checked == true) {
        $(txt).show();
        $(txt).rules("add", { required: true, messages: { required: "A value is required."} });
    }
    else {
        $(txt).rules("remove");
        $(txt).hide();
        Validation.clearErrors(txt);
    }
    if (chkClass != null) {
        validateRadioClass(chkClass);
    }
} 

function showOtherText(txt, chk, chkClass) {
    if (chk.checked == true) {
        $(txt).show();
    }
    else {
        $(txt).hide();
    }
}

function validateRadioClass(name) {
    $('.' + name).each(function (index) {
        Validation.clearErrors('#' + $(this).attr('id'));
    });

    Validation.clearErrors('#' + name);
}

var Validation = {
    clearErrors: function (elementId) {
        $(elementId).parent().children('.input-validation-error').addClass('input-validation-valid');
        $(elementId).parent().children('.input-validation-error').removeClass('input-validation-error');         //Removes validation message after input-fields      
        $(elementId).parent().children('.field-validation-error').addClass('field-validation-valid');
        $(elementId).parent().children('.field-validation-error').removeClass('field-validation-error');         //Removes validation summary   
        $(elementId + ' .field-validation-valid').html('');
        //$('.validation-summary-errors').addClass('validation-summary-valid');
        //$('.validation-summary-errors').removeClass('validation-summary-errors');
    },

    validateInput: function (elementId) {
        $(elementId).blur(function () {
            var txt = $(this).val().toUpperCase()
            txt = txt.replace(/[^a-zA-Z 0-9]+/g, '');

            $(this).val(txt);
            $(this).valid();
        });
    },

    traverseInput: function (event) {
        if ($(this).val().length == event.data.length) {
            $(event.data.nextInput).select();
        }
    },

    markRequired: function () {

        $("<span style='margin-left:4px' class='error'>*</span>").insertAfter('.requiredField');
    },

    markRequiredFields: function (parent) {
        var id = parent + ' .requiredField';
        $("<span style='margin-left:4px' class='error'>*</span>").insertAfter(id);
    },

    addRules: function () {
        //account rules
        var messagePercent = 'Please enter a valid Percent (1-99)';
        jQuery.validator.addMethod("percent", function (number, element) {
            var valid = true;
            var total = parseInt(number);
            valid = total != NaN && number >= 1 && number <= 99 && number.match(/^\d+$/);
            return valid;
        }, messagePercent);

        var messageNumber = 'Please enter a valid number';
        jQuery.validator.addMethod("number", function (number, element) {
            var valid = true;
            var total = parseInt(number);
            valid = total != NaN && number >= 1 && number.match(/^\d+$/);
            return valid;
        }, messageNumber);


        var messageRouting = 'Please enter a valid 9 digit routing number';
        jQuery.validator.addMethod("routing", function (number, element) {
            var valid = true;
            valid = number.length == 9 && number.match(/^\d+$/) && !number.match(/^5\d+$/);
            return valid;
        }, messageRouting);

        //address rules
        jQuery.validator.addMethod("invalidZip", function (zipcode, element) {
            return false;
        }, "Please enter a valid zip code");

        jQuery.validator.addMethod("invalidCity", function (city, element) {

            var d = $(element).data('tag');
            if (city == 'none' && d.Required == true) {
                return false;
            }
            else {
                return true;
            }
        }, "Please select a city");

        jQuery.validator.addMethod("invalidPhysicalCity", function (city, element) {
            var d = $(element).data('tag');
            if (city == '' && d.Required == true) {
                return false;
            }
            else {
                return true;
            }
        }, "Please enter a city");

        //FEIN rules
        var messageFEIN = 'Please enter a valid Federal Id Number';
        jQuery.validator.addMethod("FEIN1", function (phone_number, element) {
            var valid = true;
            var d = $('#' + element.id).data('tag');
            var number = $(d.SSN1).val() + $(d.SSN2).val();
            $(d.SSN).val(number);
            if (number.length == 0 && !d.Required) {
                return true;
            }
            var total = parseInt(number);
            valid = total != NaN && number.length == 9 && number.match(/^\d+$/);
            d.DisplayFEIN();
            return valid;
        }, messageFEIN);

        //phone rules
        var messageValidPhone = 'Please enter a valid number';
        jQuery.validator.addMethod("phoneUS", function (phone_number, element) {
            var valid = true;
            var d = $('#' + element.id).data('phone');
            var number = $(d.Phone1).val() + $(d.Phone2).val() + $(d.Phone3).val();
            $(d.Phone).val(number);
            if (number.length == 0 && !d.Required) {
                return true;
            }
            var total = parseInt(number);
            valid = total != NaN && number.length == 10 && number.match(/^\d+$/);
            return valid;
        }, messageValidPhone);

        //SSN rules
        var messageValidSocial = 'Please enter a valid Social Security Number';
        jQuery.validator.addMethod("socialSSN", function (phone_number, element) {
            var valid = true;
            var d = $('#' + element.id).data('tag');
            var number = $(d.SSN1).val() + $(d.SSN2).val() + $(d.SSN3).val();
            $(d.SSN).val(number);
            if (number.length == 0 && !d.Required) {
                return true;
            }
            var total = parseInt(number);
            valid = total != NaN && number.length == 9 && number.match(/^\d+$/);
            d.DisplaySSN();
            return valid;
        }, messageValidSocial);

        jQuery.validator.addMethod("submissionType", function (submissionId, element) {
            var valid = true;
            var number = $("#UserSubmissionTypeId").val();
            valid = number > 0;
            return valid;
        }, "Please select a Submission Type");

    }
}


$(document).ready(function () {


});