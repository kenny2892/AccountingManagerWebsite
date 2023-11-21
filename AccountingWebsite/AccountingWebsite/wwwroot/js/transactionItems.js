let itemOptions = [];
let itemOptionIds = [];
let measureOptions = [];
let measureOptionIds = [];

function CreateRow(extraClass)
{
	let row = document.createElement("tr");
	row.classList.add("transaction-entry-row");

	if(extraClass !== undefined && extraClass != "")
	{
		row.classList.add(extraClass);
	}

	return row;
}

function CreateIdHiddenInput(transactionEntryId, modEntryId, parentModEntryId)
{
	let idInput = document.createElement("input");
	idInput.type = "hidden";
	idInput.id = "idValues";

	idInput.setAttribute("transaction-entry-id", transactionEntryId);
	idInput.setAttribute("mod-entry-id", modEntryId);
	idInput.setAttribute("parent-mod-entry-id", parentModEntryId);

	return idInput;
}

function CreateNameColumn(nameVal, nameId)
{
	let name = document.createElement("td");
	name.classList.add("transaction-entry-item");
	name.append(CreateDropdownSearch(nameId, "Select an Item", nameVal, itemOptions, itemOptionIds));

	return name;
}

function CreateNameSubColumn(nameVal, nameId)
{
	let name = document.createElement("td");
	name.classList.add("transaction-entry-item");
	name.append("↳");
	name.append(CreateDropdownSearch(nameId, "Select an Item", nameVal, itemOptions, itemOptionIds));

	return name;
}

function CreateQtyColumn(qtyVal)
{
	let qty = document.createElement("td");
	qty.classList.add("transaction-entry-qty");
	let qtyInput = document.createElement("input");
	qtyInput.type = "text";
	qtyInput.id = "itemQty";
	SetNumberInputFilter(qtyInput);
	qty.append(qtyInput);

	if(qtyVal !== undefined && qtyVal != "")
	{
		qtyInput.value = qtyVal;
	}

	return qty;
}

function CreateCostColumn(costVal)
{
	let cost = document.createElement("td");
	cost.classList.add("transaction-entry-cost");
	let costInput = document.createElement("input");
	costInput.type = "text";
	costInput.id = "itemCost";
	SetNumberInputFilter(costInput);
	cost.append(costInput);

	if(costVal !== undefined && costVal != "")
	{
		costInput.value = costVal;
	}

	return cost;
}

// Source: https://stackoverflow.com/a/469362
function SetNumberInputFilter(textbox)
{
	["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop", "focusout"].forEach(function (event)
	{
		textbox.addEventListener(event, function (e)
		{
			if(/^-{0,1}\d*\.?\d*$/.test(this.value))
			{
				// Accepted value.
				if(["keydown", "mousedown", "focusout"].indexOf(e.type) >= 0)
				{
					this.classList.remove("input-error");
					this.setCustomValidity("");
				}

				this.oldValue = this.value;
				this.oldSelectionStart = this.selectionStart;
				this.oldSelectionEnd = this.selectionEnd;
			}

			else if(this.hasOwnProperty("oldValue"))
			{
				// Rejected value: restore the previous one.
				this.classList.add("input-error");
				this.setCustomValidity("Enter a valid number");
				this.reportValidity();
				this.value = this.oldValue;
				this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
			}

			else
			{
				// Rejected value: nothing to restore.
				this.value = "";
			}
		});
	});
}

function CreateMeasureColumn(measVal, measId)
{
	let measure = document.createElement("td");
	measure.classList.add("transaction-entry-measurement");
	measure.append(CreateDropdownSearch(measId, "Select a Measure", measVal, measureOptions, measureOptionIds));

	return measure;
}

function CreateNoteColumn(noteVal)
{
	let note = document.createElement("td");
	note.classList.add("transaction-entry-note");
	let noteInput = document.createElement("input");
	noteInput.type = "text";
	noteInput.id = "itemNote";

	if(noteVal !== undefined && noteVal != "")
	{
		noteInput.value = noteVal;
	}

	note.append(noteInput);

	return note;
}

function AddItemRow(entryId, nameVal, nameId, qtyVal, measVal, measId, costVal, noteVal)
{
	let row = CreateRow();
	row.append(CreateIdHiddenInput(entryId, "-1", "-1"));
	row.append(CreateNameColumn(nameVal, nameId));
	row.append(CreateQtyColumn(qtyVal));
	row.append(CreateMeasureColumn(measVal, measId));
	row.append(CreateCostColumn(costVal));
	row.append(CreateNoteColumn(noteVal));

	let buttonsCol = document.createElement("td");
	let deleteBtn = document.createElement("button");
	deleteBtn.classList.add("row-delete-button");
	deleteBtn.innerHTML = "✖";
	deleteBtn.onclick = function ()
	{
		row.classList.add("transaction-delete-row");
		$(row).hide();
	};

	let addModBtn = document.createElement("button");
	addModBtn.innerHTML = "Add Inner Item";
	addModBtn.onclick = function () { AddModRow(row, entryId, "-1") };

	buttonsCol.append(deleteBtn);
	buttonsCol.append(addModBtn);
	row.append(buttonsCol);

	$("#transactionItemsTable tbody").append(row);

	return row;
}

function AddModRow(parentRow, entryId, modifierId, nameVal, nameId, qtyVal, measVal, measId, costVal, noteVal)
{
	let row = CreateRow("transaction-parent-modifier-row");
	row.append(CreateIdHiddenInput(entryId, modifierId, "-1"));
	row.append(CreateNameSubColumn(nameVal, nameId));
	row.append(CreateQtyColumn(qtyVal));
	row.append(CreateMeasureColumn(measVal, measId));
	row.append(CreateCostColumn(costVal));
	row.append(CreateNoteColumn(noteVal));

	let buttonsCol = document.createElement("td");
	let deleteBtn = document.createElement("button");
	deleteBtn.classList.add("row-delete-button");
	deleteBtn.innerHTML = "✖";
	deleteBtn.onclick = function ()
	{
		row.classList.add("transaction-delete-row");
		$(row).hide();
	};

	let addModBtn = document.createElement("button");
	addModBtn.innerHTML = "Add Mod";
	addModBtn.onclick = function () { AddChildModRow(row, modifierId, "-1") };

	buttonsCol.append(deleteBtn);
	buttonsCol.append(addModBtn);
	row.append(buttonsCol);

	let nextItemRow = $(parentRow).nextAll(".transaction-entry-row:not(.transaction-parent-modifier-row):not(.transaction-child-modifier-row)").first();
	let rowsBetween = $(parentRow).nextUntil(nextItemRow);

	if(rowsBetween.length == 0)
	{
		parentRow.after(row);
	}

	else
	{
		rowsBetween.last().after(row);
	}

	return row;
}

function AddChildModRow(parentRow, modifierId, parentModifierId, nameVal, nameId, qtyVal, measVal, measId, costVal, noteVal)
{
	let row = CreateRow("transaction-child-modifier-row");
	row.append(CreateIdHiddenInput("-1", modifierId, parentModifierId));
	row.append(CreateNameSubColumn(nameVal, nameId));
	row.append(CreateQtyColumn(qtyVal));
	row.append(CreateMeasureColumn(measVal, measId));
	row.append(CreateCostColumn(costVal));
	row.append(CreateNoteColumn(noteVal));

	let buttonsCol = document.createElement("td");
	let deleteBtn = document.createElement("button");
	deleteBtn.classList.add("row-delete-button");
	deleteBtn.innerHTML = "✖";
	deleteBtn.onclick = function ()
	{
		row.classList.add("transaction-delete-row");
		$(row).hide();
	};
	buttonsCol.append(deleteBtn);
	row.append(buttonsCol);

	let nextItemOrParentRow = $(parentRow).nextAll(".transaction-parent-modifier-row, .transaction-entry-row:not(.transaction-child-modifier-row)").first();
	let rowsBetween = $(parentRow).nextUntil(nextItemOrParentRow);

	if(rowsBetween.length == 0)
	{
		parentRow.after(row);
	}

	else
	{
		rowsBetween.last().after(row);
	}

	return row;
}

function Save()
{
	var formData = new FormData();
	formData.append("vm.TransactionID", $("#transactionId").val());

	let index = 0;
	let hasInvalids = false;
	$(".transaction-entry-row").each(function ()
	{
		// Find Type
		if(this.classList.contains("transaction-child-modifier-row"))
		{
			formData.append("vm.EntryTypes[" + index + "]", "ChildMod");
		}

		else if(this.classList.contains("transaction-parent-modifier-row"))
		{
			formData.append("vm.EntryTypes[" + index + "]", "ParentMod");
		}

		else
		{
			formData.append("vm.EntryTypes[" + index + "]", "Item");
		}

		// Check if deleted or not
		let isDeleted = false;
		if(this.classList.contains("transaction-delete-row"))
		{
			formData.append("vm.ToDelete[" + index + "]", true);
			isDeleted = true;
		}

		else
		{
			formData.append("vm.ToDelete[" + index + "]", false);
		}

		// Get Ids
		let idValuesElement = $(this).find("#idValues")[0];
		formData.append("vm.TransactionEntryIds[" + index + "]", idValuesElement.getAttribute("transaction-entry-id"));
		formData.append("vm.ModifierIds[" + index + "]", idValuesElement.getAttribute("mod-entry-id"));
		formData.append("vm.ParentModifierIds[" + index + "]", idValuesElement.getAttribute("parent-mod-entry-id"));

		// Get Item Id
		let selectedItemId = $(this).find(".transaction-entry-item .dropdown-search-input").data("selectedId");
		let selectedItemIdToUse = selectedItemId == "" || selectedItemId === undefined ? "-1" : selectedItemId;

		if(!isDeleted && selectedItemId != selectedItemIdToUse)
		{
			hasInvalids = true;
		}

		formData.append("vm.ItemIds[" + index + "]", selectedItemIdToUse);

		// Get Quantity
		let qty = $(this).find(".transaction-entry-qty input")[0].value == "" ? "0" : $(this).find(".transaction-entry-qty input")[0].value;
		formData.append("vm.Quantities[" + index + "]", qty);

		// Get Measure Id
		let selectedMeasureId = $(this).find(".transaction-entry-measurement .dropdown-search-input").data("selectedId");
		let selectedMeasureIdToUse = selectedMeasureId == "" || selectedMeasureId === undefined ? "-1" : selectedMeasureId;

		if(!isDeleted && selectedMeasureId != selectedMeasureIdToUse)
		{
			hasInvalids = true;
		}

		formData.append("vm.MeasurementIds[" + index + "]", selectedMeasureIdToUse);

		// Get Cost
		let cost = $(this).find(".transaction-entry-cost input")[0].value == "" ? "0" : $(this).find(".transaction-entry-cost input")[0].value;
		formData.append("vm.Costs[" + index + "]", cost);

		// Get Note
		formData.append("vm.Notes[" + index++ + "]", $(this).find(".transaction-entry-note input")[0].value);
	});

	if(hasInvalids)
	{
		$("#saveStatus").html("Please verify you selection.");
		$("#saveStatus").removeClass("success");
		$("#saveStatus").addClass("error");
		$("#saveStatus").css("display", "block");
		$("#saveStatus").fadeOut(5000, "swing");
		return;
	}

	$.ajax
	({
		type: "POST",
		url: "/Transactions/UpdateTransactionItems",
		data: formData,
		processData: false,
		contentType: false,
		success: function (value)
		{
			if(value) 
			{
				$("#saveStatus").html("Transaction Items Saved!");
				$("#saveStatus").addClass("success");
				$("#saveStatus").removeClass("error");
				$("#saveStatus").css("display", "block");
				$("#saveStatus").fadeOut(5000, "swing");
			}
		},
		error: function (textStatus, errorThrown)
		{
			$("#saveStatus").html("An Error has occured.");
			$("#saveStatus").removeClass("success");
			$("#saveStatus").addClass("error");
			$("#saveStatus").css("display", "block");
			$("#saveStatus").fadeOut(5000, "swing");
			console.log(textStatus)
			console.log(errorThrown)
		}

	});
}

function SetupItemMeasurmentPopup()
{
	SetNumberInputFilter($("#newMeasureAmountInput")[0]);
	let popupMeasureDropdown = CreateDropdownSearch("", "Select Inner Measure", "", measureOptions, measureOptionIds);
	popupMeasureDropdown.id = "newMeasureInnerMeasureInput";
	$(".pop-up-measure-dropdown-wrapper").append(popupMeasureDropdown);
	$(popupMeasureDropdown).hide();

	$('#newMeasureIsCaseInput').change(function () { ShowInnerMeasureSelection(); });
	$('#newMeasureIsContainerInput').change(function () { ShowInnerMeasureSelection(); });

	$("#itemMeasurementPopup .pop-up-page-overlay").click(function () { HideItemMeasurementPopup(); });
	$("#itemMeasurementPopup .pop-up-content").click(function () { HideItemMeasurementPopup(); });
	$("#itemMeasurementPopup .pop-up-item-wrapper").click(function (event) { event.stopPropagation(); });
	$("#itemMeasurementPopup .pop-up-measure-wrapper").click(function (event) { event.stopPropagation(); });
	$("#itemMeasurementPopup .pop-up .receipt-edit-img").click(function (event) { event.stopPropagation(); });

	$("#itemMeasurementPopup .pop-up-search").on('input', function ()
	{
		let searchValue = this.value;
		let optionsWrapper = $(this).parent().next(".pop-up-existing-wrapper");
		$(optionsWrapper).find("li").each(function ()
		{
			if(searchValue == "" || this.innerHTML.toLowerCase().includes(searchValue.toLowerCase()))
			{
				$(this).css("display", "block");
			}

			else
			{
				$(this).css("display", "none");
			}
		});
	});
}

function SetupImportPopup()
{
	$("#importPopup .pop-up-page-overlay").click(function () { HideImportPopup(); });
}

function PopulateExistingItemPopup()
{
	$.ajax
	({
		type: "POST",
		url: "/Transactions/GetItemNames",
		success: function (value)
		{
			if(value) 
			{
				$(".pop-up-item-wrapper .pop-up-existing-wrapper ul").remove();
				let ul = document.createElement("ul");

				for(let i = 0; i < value.length; i++)
				{
					let li = document.createElement("li");
					li.innerHTML = value[i];
					ul.append(li);
				}

				$(".pop-up-item-wrapper .pop-up-existing-wrapper").append(ul);
			}
		},
		error: function (textStatus, errorThrown)
		{
			console.log(textStatus)
			console.log(errorThrown)
		}
	});
}

function PopulateExistingMeasurePopup()
{
	$.ajax
	({
		type: "POST",
		url: "/Transactions/GetMeasureNames",
		success: function (value)
		{
			if(value) 
			{
				$(".pop-up-measure-wrapper .pop-up-existing-wrapper ul").remove();
				let ul = document.createElement("ul");

				for(let i = 0; i < value.length; i++)
				{
					let li = document.createElement("li");
					li.innerHTML = value[i];
					ul.append(li);
				}

				$(".pop-up-measure-wrapper .pop-up-existing-wrapper").append(ul);
			}
		},
		error: function (textStatus, errorThrown)
		{
			console.log(textStatus)
			console.log(errorThrown)
		}
	});
}

function ShowInnerMeasureSelection()
{
	let isCase = $("#newMeasureIsCaseInput").is(':checked');
	let isContainer = $("#newMeasureIsContainerInput").is(':checked');

	if(isCase || isContainer)
	{
		$("#newMeasureInnerMeasureInput").show();
	}

	else
	{
		$("#newMeasureInnerMeasureInput").hide();
	}
}

function ShowItemMeasurementPopup()
{
	PopulateExistingItemPopup();
	PopulateExistingMeasurePopup();
	$("#itemMeasurementPopup").css("display", "initial");
}

function HideItemMeasurementPopup()
{
	$("#itemMeasurementPopup").css("display", "none");
}

function CreateItem()
{
	var formData = new FormData();
	formData.append("name", $("#newItemNameInput").val());
	formData.append("note", $("#newItemNoteInput").val());

	$.ajax
	({
		type: "POST",
		url: "/Transactions/CreateTransactionItem",
		data: formData,
		processData: false,
		contentType: false,
		success: function (value)
		{
			if(value) 
			{
				PopulateExistingItemPopup();
				$("#popupItemCreateStatus").addClass("success");
				$("#popupItemCreateStatus").removeClass("error");
				$("#popupItemCreateStatus").html("Item Created!");
				$("#popupItemCreateStatus").css("display", "block");
				$("#popupItemCreateStatus").fadeOut(5000, "swing");
				UpdateItemDropdowns();
				$("#newItemNameInput").val("");
				$("#newItemNoteInput").val("");
			}
		},
		error: function (textStatus, errorThrown)
		{
			$("#popupItemCreateStatus").removeClass("success");
			$("#popupItemCreateStatus").addClass("error");
			$("#popupItemCreateStatus").html("An Error has occured.");
			$("#popupItemCreateStatus").css("display", "block");
			$("#popupItemCreateStatus").fadeOut(5000, "swing");
			console.log(textStatus)
			console.log(errorThrown)
		}
	});
}

function CreateMeasure()
{
	var formData = new FormData();
	formData.append("name", $("#newMeasureNameInput").val());
	formData.append("shortname", $("#newMeasureShortNameInput").val());
	formData.append("isCase", $("#newMeasureIsCaseInput").is(':checked'));
	formData.append("isContainer", $("#newMeasureIsContainerInput").is(':checked'));
	formData.append("amount", $("#newMeasureAmountInput").val());
	formData.append("innerMeasureId", $("#newMeasureInnerMeasureInput").find(".dropdown-search-input").data("selectedId"));

	$.ajax
	({
		type: "POST",
		url: "/Transactions/CreateMeasure",
		data: formData,
		processData: false,
		contentType: false,
		success: function (value)
		{
			if(value) 
			{
				PopulateExistingMeasurePopup();
				$("#popupMeasureCreateStatus").addClass("success");
				$("#popupMeasureCreateStatus").removeClass("error");
				$("#popupMeasureCreateStatus").html("Measure Created!");
				$("#popupMeasureCreateStatus").css("display", "block");
				$("#popupMeasureCreateStatus").fadeOut(5000, "swing");
				UpdateMeasureDropdowns();
			}
		},
		error: function (textStatus, errorThrown)
		{
			$("#popupMeasureCreateStatus").removeClass("success");
			$("#popupMeasureCreateStatus").addClass("error");
			$("#popupMeasureCreateStatus").html("An Error has occured.");
			$("#popupMeasureCreateStatus").css("display", "block");
			$("#popupMeasureCreateStatus").fadeOut(5000, "swing");
			console.log(textStatus)
			console.log(errorThrown)
		}
	});
}

function UpdateItemDropdowns()
{
	let newItemNames = [];
	let newItemIds = [];

	$.ajax
	({
		type: "POST",
		url: "/Transactions/GetItemNames",
		success: function (value)
		{
			if(value) 
			{
				newItemNames = value;

				$.ajax
				({
					type: "POST",
					url: "/Transactions/GetItemIds",
					success: function (value)
					{
						if(value) 
						{
							newItemIds = value;

							itemOptions = newItemNames;
							itemOptionIds = newItemIds;

							$(".transaction-entry-item").find(".dropdown-search").each(function ()
							{
								ReplaceOptions(this, newItemNames, newItemIds);
							});
						}
					},
					error: function (textStatus, errorThrown)
					{
						console.log(textStatus);
						console.log(errorThrown);
						return;
					}
				});
			}
		},
		error: function (textStatus, errorThrown)
		{
			console.log(textStatus);
			console.log(errorThrown);
			return;
		}
	});
}

function UpdateMeasureDropdowns()
{
	let newMeasureNames = [];
	let newMeasureIds = [];

	$.ajax
	({
		type: "POST",
		url: "/Transactions/GetMeasureNames",
		success: function (value)
		{
			if(value) 
			{
				newMeasureNames = value;

				$.ajax
				({
					type: "POST",
					url: "/Transactions/GetMeasureIds",
					success: function (value)
					{
						if(value) 
						{
							newMeasureIds = value;

							$(".transaction-entry-measurement").find(".dropdown-search").each(function ()
							{
								ReplaceOptions(this, newMeasureNames, newMeasureIds);
							});

							ReplaceOptions($("#newMeasureInnerMeasureInput"), newMeasureNames, newMeasureIds);
						}
					},
					error: function (textStatus, errorThrown)
					{
						console.log(textStatus);
						console.log(errorThrown);
						return;
					}
				});
			}
		},
		error: function (textStatus, errorThrown)
		{
			console.log(textStatus);
			console.log(errorThrown);
			return;
		}
	});
}

function ShowImportPopup()
{
	$("#importPopup").css("display", "initial");
}

function HideImportPopup()
{
	$("#importPopup").css("display", "none");
}

function CopyItems()
{
	let allRows = {};
	let index = 0;
	let hasInvalids = false;
	$(".transaction-entry-row").each(function ()
	{
		if(!this.classList.contains("transaction-delete-row"))
		{
			let row = [];
			row[0] = "";

			// Find Type
			if(this.classList.contains("transaction-child-modifier-row"))
			{
				row[0] = "ChildMod";
			}

			else if(this.classList.contains("transaction-parent-modifier-row"))
			{
				row[0] = "ParentMod";
			}

			else
			{
				row[0] = "Item";
			}

			// Get Ids
			let idValuesElement = $(this).find("#idValues")[0];
			row[1] = idValuesElement.getAttribute("transaction-entry-id");
			row[2] = idValuesElement.getAttribute("mod-entry-id");
			row[3] = idValuesElement.getAttribute("parent-mod-entry-id");

			// Get Item Id
			let selectedItemId = $(this).find(".transaction-entry-item .dropdown-search-input").data("selectedId");
			row[4] = selectedItemId == "" || selectedItemId === undefined ? "-1" : selectedItemId;

			if(selectedItemId != row[4])
			{
				hasInvalids = true;
			}

			// Get Item Name
			row[5] = $(this).find(".transaction-entry-item .dropdown-search-input").val();

			// Get Quantity
			row[6] = $(this).find(".transaction-entry-qty input")[0].value == "" ? "0" : $(this).find(".transaction-entry-qty input")[0].value;

			// Get Measure Id
			let selectedMeasureId = $(this).find(".transaction-entry-measurement .dropdown-search-input").data("selectedId");
			row[7] = selectedMeasureId == "" || selectedMeasureId === undefined ? "-1" : selectedMeasureId;

			if(selectedMeasureId != row[7])
			{
				hasInvalids = true;
			}

			// Get Measurement Name
			row[8] = $(this).find(".transaction-entry-measurement .dropdown-search-input").val();

			// Get Cost
			row[9] = $(this).find(".transaction-entry-cost input")[0].value == "" ? "0" : $(this).find(".transaction-entry-cost input")[0].value;

			// Get Note
			row[10] = $(this).find(".transaction-entry-note input")[0].value;

			if(hasInvalids)
			{
				return false;
			}

			allRows[index++] = row;
		}
	});

	if(hasInvalids)
	{
		$("#saveStatus").html("Please verify you selection.");
		$("#saveStatus").removeClass("success");
		$("#saveStatus").addClass("error");
		$("#saveStatus").css("display", "block");
		$("#saveStatus").fadeOut(5000, "swing");
		return;
	}

	navigator.clipboard.writeText(JSON.stringify(allRows));
	$("#saveStatus").html("Items Copied!");
	$("#saveStatus").addClass("success");
	$("#saveStatus").removeClass("error");
	$("#saveStatus").css("display", "block");
	$("#saveStatus").fadeOut(5000, "swing");
}

function PasteItems()
{
	let importText = $(".pop-up-import-input").val();
	let allRows = JSON.parse(importText);
	let lastItemRow = {};
	let lastParentModRow = {};
	let lastChildModRow = {};

	for(let [key, value] of Object.entries(allRows))
	{
		let type = value[0];
		let entryId = value[1];
		let modId = value[2];
		let parentModId = value[3];
		let itemId = value[4];
		let itemName = value[5];
		let qty = value[6];
		let measurementId = value[7];
		let measurementName = value[8];
		let cost = value[9];
		let note = value[10];

		if(type == "Item")
		{
			lastItemRow = AddItemRow('-1', itemName, itemId, qty, measurementName, measurementId, cost, note);
		}

		else if(type == "ParentMod")
		{
			lastParentModRow = AddModRow(lastItemRow, '-1', '-1', itemName, itemId, qty, measurementName, measurementId, cost, note);
		}

		else if(type == "ChildMod")
		{
			lastChildModRow = AddChildModRow(lastParentModRow, '-1', '-1', itemName, itemId, qty, measurementName, measurementId, cost, note);
		}
	}

	$(".pop-up-import-input").val("");
	HideImportPopup();
}