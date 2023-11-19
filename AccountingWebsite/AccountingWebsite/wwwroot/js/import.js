function ImportFile(file)
{
	var formData = new FormData();

	var fileInput = $("#file-uploader")[0];
	var file = fileInput.files[0];
	formData.append("file", file)
	formData.append("isCredit", $("#is-credit").is(":checked"))
	formData.append("bank", $("#bank").val())

	$.ajax
	({
		url: "/Transactions/UploadFile",
		data: formData,
		processData: false,
		contentType: false,
		type: "POST",
		success: function (value)
		{
			if(value) 
			{
				$("#import-results").append(value);
				$("#is-credit-container").hide();
				$("#bank-container").hide();
				$("#file-uploader").hide();
			}
		},
	});
}

// Setup File Import Button
$("#file-uploader").change(function ()
{
	let fileUploader = document.getElementById("file-uploader");

	if(fileUploader.value == "")
	{
		return;
	}

	let file = fileUploader.files[0];
	ImportFile(fileUploader);
});