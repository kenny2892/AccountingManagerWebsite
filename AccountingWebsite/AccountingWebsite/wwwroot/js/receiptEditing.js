function FindMatches(displayIndex)
{
	var formData = new FormData();
	formData.append("vm.FileName", $("#file-name-" + displayIndex).val());
	formData.append("vm.Vendor", $("#vendor-input-" + displayIndex).find(":selected").val());
	formData.append("vm.PurchaseDate", $("#purchase-date-input-" + displayIndex).val());
	formData.append("vm.Amount", $("#amount-input-" + displayIndex).val());

	$.ajax
	({
		url: "/Receipts/FindMatches",
		data: formData,
		processData: false,
		contentType: false,
		type: "POST",
		success: function (value)
		{
			if(value) 
			{
				$("#transactionTable-body-" + displayIndex + " tr").remove();
				$("#transactionTable-body-" + displayIndex).append(value);
				SetUpTransactionSelection(displayIndex);
			}
		},
	});
}

function SetUpTransactionSelection(displayIndex)
{
	// Set up the checkbox click
	$("#transactionTable-body-" + displayIndex + " .transaction-selection-checkbox").click(function ()
	{
		$("#transactionTable-body-" + displayIndex + " .transaction-selection-checkbox").not(this).prop('checked', false);

		if($(this).is(':checked'))
		{
			$("#selected-transaction-id-" + displayIndex).val($(this).val());
		}

		else
		{
			$("#selected-transaction-id-" + displayIndex).val("");
		}
	})

	// Update incase a checkbox is already clicked
	if($("#transactionTable-body-" + displayIndex + " .transaction-selection-checkbox:checked").length == 1)
	{
		$("#selected-transaction-id-" + displayIndex).val($("#transactionTable-body-" + displayIndex + " .transaction-selection-checkbox:checked").val());
	}

	// Link the whole row
	// Source: https://stackoverflow.com/a/69202407 & https://stackoverflow.com/a/47043715
	$('tr').off('click').on('click', function (event)
	{
		var $target = $(event.target);
		if(!$target.is('input:checkbox'))
		{
			$(this).find('.transaction-selection-checkbox').each(function ()
			{
				this.click();
			})
		}
	});
}

function ValidateSubmit(event, numberOfReceipts)
{
	// Check if each Receipt has Valid data
	for(var i = 0; i < numberOfReceipts; i++)
	{
		if($("#selected-transaction-id-" + i).val() == "" || $("#purchase-date-input-" + i).val() == "1990-01-01T12:00")
		{
			event.preventDefault();
			return false;
		}
	}

	return true;
}