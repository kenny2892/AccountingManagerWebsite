function SetupTheme(dropdownId)
{
    let currentTheme = "Dark";

    $.ajax
    ({
		url: "/Base/GetThemeName",
        success: function(value)
		{
            if(value != null && value != "")
            {
                currentTheme = value;
            }
        },
    })
    .done(function()
    {
		document.documentElement.setAttribute('data-theme', currentTheme);
		SelectTheme(currentTheme, dropdownId);
    });
}

function SelectTheme(currentTheme, dropdownId)
{
	let dropdown = document.getElementById(dropdownId);

	$.ajax
    ({
		url: "/Base/GetThemeIndex",
		data: {theme: currentTheme},
        success: function(value)
		{
			dropdown.value = value;
        },
	});

	dropdown.onchange = function ()
	{
		SetTheme(dropdown.options[dropdown.selectedIndex].text);
	};
}

function SetTheme(themeToSetTo)
{
	$.ajax
    ({
		url: "/Base/SetTheme",
		data: {themeToSetTo: themeToSetTo},
        success: function(value)
		{
            if(value)
			{
				document.documentElement.setAttribute('data-theme', themeToSetTo);
            }
        },
    });
}