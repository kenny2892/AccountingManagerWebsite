$('.dropdown-three-stage .anchor').click(function ()
{
	if($(this).closest(".dropdown-three-stage").hasClass('visible'))
	{
		$(this).closest(".dropdown-three-stage").removeClass('visible');
	}

	else
	{
		$(this).closest(".dropdown-three-stage").addClass('visible');
	}
});