function CreateDropdownSearch(selectedId, promptText, selectedOption, options, optionIds)
{
	let dropdownWrapper = document.createElement("div");
	dropdownWrapper.tabIndex = 100;
	dropdownWrapper.classList.add("dropdown-search");

	let textInput = document.createElement("input");
	textInput.classList.add("dropdown-search-input");
	textInput.type = "text";
	textInput.placeholder = promptText;

	if(selectedOption !== undefined && selectedOption != "")
	{
		textInput.value = selectedOption;
	}

	if(selectedId !== undefined && selectedId != "")
	{
		$(textInput).data("selectedId", selectedId);
	}

	let listWrapper = document.createElement("ul");
	listWrapper.tabIndex = 0;
	listWrapper.classList.add("dropdown-search-elements");

	for(var i = 0; i < options.length; i++)
	{
		let option = document.createElement("li");
		option.tabIndex = 0;
		option.innerHTML = options[i];
		option.id = optionIds[i];
		listWrapper.append(option);
	}

	dropdownWrapper.append(textInput);
	dropdownWrapper.append(listWrapper);
	SetupDropdownSearch(dropdownWrapper);

	return dropdownWrapper;
}

function CheckForFocus(element, hasFocus)
{
	if(hasFocus)
	{
		return true;
	}

	$(element).children().each(function ()
	{
		if($(this).is(':focus'))
		{
			hasFocus = true;
		}

		hasFocus = CheckForFocus($(this), hasFocus);

		if(hasFocus)
		{
			return false;
		}
	});

	return hasFocus;
}

function SetupDropdownSearch(dropdown)
{
	$(dropdown).focusin(function ()
	{
		$(this).find(".dropdown-search-elements").css("display", "block");
		UpdateSearch(this);
	});

	$(dropdown).focusout(function ()
	{
		let dropdown = this;
		setTimeout(function ()
		{
			HideDropdownElements(dropdown);
		}, 100);
	});

	$(dropdown).find('.dropdown-search-elements li').click(function ()
	{
		var dropdown = $(this).closest(".dropdown-search");
		dropdown.find(".dropdown-search-input").val(this.innerHTML);
		dropdown.blur();
	});

	$(dropdown).find(".dropdown-search-input").on('input', function ()
	{
		UpdateSearch($(this).closest(".dropdown-search"));
	});
}

function ReplaceOptions(dropdown, newOptions, newOptionIds)
{
	let currSelectedOption = $(dropdown).find(".dropdown-search-input").val();
	let selectedId = newOptions.indexOf(currSelectedOption);

	if(selectedId < 0)
	{
		$(dropdown).find(".dropdown-search-input").val("");
	}

	if(selectedId !== undefined && selectedId != "")
	{
		$(dropdown).find(".dropdown-search-input").data("selectedId", selectedId);
	}

	let listWrapper = $(dropdown).find(".dropdown-search-elements")[0];

	$(listWrapper).find("li").remove();
	for(var i = 0; i < newOptions.length; i++)
	{
		let option = document.createElement("li");
		option.tabIndex = 0;
		option.innerHTML = newOptions[i];
		option.id = newOptionIds[i];
		listWrapper.append(option);

		$(option).click(function ()
		{
			var dropdown = $(this).closest(".dropdown-search");
			dropdown.find(".dropdown-search-input").val(this.innerHTML);
			dropdown.blur();
		});
	}
}

function HideDropdownElements(dropdown)
{
	// Check each child element to see if it is still in focus
	let currValue = $(dropdown).find(".dropdown-search-input")[0].value;
	let stillInFocus = CheckForFocus($(this), false);

	if(!stillInFocus)
	{
		setTimeout(function ()
		{
			$(dropdown).find(".dropdown-search-elements").css("display", "none");
		}, 100);

		// Check if the value is valid
		let selectedId = "";
		let updatedText = "";
		$(dropdown).find(".dropdown-search-elements").children().each(function ()
		{
			if($(this).html().toLowerCase() == currValue.toLowerCase())
			{
				selectedId = this.id;
				updatedText = $(this).html();
				return false;
			}
		});

		$(dropdown).find(".dropdown-search-input").val(updatedText);
		$(dropdown).find(".dropdown-search-input").data("selectedId", selectedId);
	}
}

function UpdateSearch(dropdownToUpdate)
{
	// Get search value
	let searchValue = $(dropdownToUpdate).find(".dropdown-search-input").val();

	// Hide any options that do not match
	$(dropdownToUpdate).find('.dropdown-search-elements').children().each(function ()
	{
		if(searchValue == "" || $(this).html().toLowerCase().includes(searchValue.toLowerCase()))
		{
			$(this).css("display", "block");
		}

		else
		{
			$(this).css("display", "none");
		}
	});
}