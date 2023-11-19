let isLoading = false;
let finished = false;
let methodUrl = "";
let formId = "";
let tableId = "";
let viewModelName = "";

function Search(event)
{
	if(event.key === 'Enter')
	{
		finished = false;
		$("input[name='" + viewModelName + ".CurrentRowCount']").val("0");
		window.scrollTo(0, 0);
		LoadTableContents();
	}
}

function ReloadTable(newToSortBy)
{
	if($("input[name='" + viewModelName + ".SortBy']").val() == newToSortBy)
	{
		if($("input[name='" + viewModelName + ".IsDescending']").val().toLowerCase() == "true")
		{
			$("input[name='" + viewModelName + ".IsDescending']").val("false");
		}

		else
		{
			$("input[name='" + viewModelName + ".IsDescending']").val("true");
		}
	}

	else
	{
		$("input[name='" + viewModelName + ".IsDescending']").val("true");
	}

	finished = false;
	LoadTableContents();
}

function LoadTableContents()
{
	$.ajax
	({
		type: "POST",
		url: methodUrl,
		data: $("#" + formId).serialize(),
		success: function (value)
		{
			if(value) 
			{
				let initialTableRowCount = $("#" + tableId + " tr").length + 1 - 1;
				$("#" + tableId + " tbody").remove();
				$("#" + tableId + " thead").remove();
				$("#" + tableId).append(value);

				if(initialTableRowCount == $("#" + tableId + " tr").length)
				{
					finished = true;
				}
			}
		},
		error: function (textStatus, errorThrown)
		{
			console.log(textStatus)
			console.log(errorThrown)
		}

	});
}

function SetUpInfiniteScroll()
{
	window.onscroll = function ()
	{
		if((window.innerHeight + window.scrollY) >= document.body.offsetHeight)
		{
			if(!isLoading && !finished)
			{
				isLoading = true;
				LoadTableContents();
				setTimeout(() => { isLoading = false; }, "500");
			}
		}
	};
}