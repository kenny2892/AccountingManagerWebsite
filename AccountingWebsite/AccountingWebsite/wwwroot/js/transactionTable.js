let isLoading = false;

function Search(event, prevSortBy, prevSortOrder)
{
	if(event.key === 'Enter')
	{
		ReloadTable("default", prevSortBy, prevSortOrder);
	}
}

function ReloadTable(sortBy, prevSortBy, prevSortOrder)
{
	LoadTableContents(-1, 'transactionTable', sortBy, prevSortBy, prevSortOrder, $("#bank-search").val(), $("#purchasing-date-search").val(),
		$("#posting-date-search").val(), $("#transaction-date-search").val(), $("#amount-search").val(), $("#description-search").val(),
		$("#category-one-search").val(), $("#category-two-search").val(), $("#type-search").val(), $("#vendor-name-search").val(), $("#is-purchase-search").val());
}

function LoadTableContents(existingRowCount, tableId, sortByParam, prevSortByParam, prevSortOrderParam,
	bankSearchParam, purchasingDateSearchParam, postingDateSearchParam, transactionTypeSearchParam, amountSearchParam, descriptionSearchParam,
        categoryOneSearchParam, categoryTwoSearchParam, typeSearchParam, vendorNameSearchParam, isPurchaseSearchParam)
{
	$.ajax
	({
		url: "/Transactions/LoadTransactions",
		data: {
			'existingRowCount': existingRowCount, 'sortBy': sortByParam, 'prevSortBy': prevSortByParam, 'prevSortOrder': prevSortOrderParam,
			'bankSearch': bankSearchParam, 'purchasingDateSearch': purchasingDateSearchParam, 'postingDateSearch': postingDateSearchParam,
			'transactionTypeSearch': transactionTypeSearchParam, 'amountSearch': amountSearchParam, 'descriptionSearch': descriptionSearchParam,
			'categoryOneSearch': categoryOneSearchParam, 'categoryTwoSearch': categoryTwoSearchParam, 'typeSearch': typeSearchParam,
			'vendorNameSearch': vendorNameSearchParam, 'isPurchaseSearch': isPurchaseSearchParam
		},
		success: function (value)
		{
			if(value) 
			{
				$("#" + tableId + " tbody").remove();
				$("#" + tableId + " thead").remove();
				$("#" + tableId).append(value);
			}
		},
	});
}

function SetUpInfiniteScroll(prevSortBy, prevSortOrder)
{
	window.onscroll = function ()
	{
		if((window.innerHeight + window.scrollY) >= document.body.offsetHeight)
		{
			if(!isLoading)
			{
				isLoading = true;
				LoadTableContents($('#transactionTable tr').length, 'transactionTable', 'default', prevSortBy, prevSortOrder, $("#bank-search").val(), $("#purchasing-date-search").val(),
					$("#posting-date-search").val(), $("#transaction-date-search").val(), $("#amount-search").val(), $("#description-search").val(),
					$("#category-one-search").val(), $("#category-two-search").val(), $("#type-search").val(), $("#vendor-name-search").val(), $("#is-purchase-search").val());
				isLoading = false;
			}
		}
	};
}