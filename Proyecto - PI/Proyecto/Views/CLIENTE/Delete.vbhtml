@ModelType Proyecto.CLIENTE
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>CLIENTE</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.tel)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.tel)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.nombre)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.nombre)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.primerApellido)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.primerApellido)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.segundoApellido)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.segundoApellido)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.correo)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.correo)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.distrito)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.distrito)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.canton)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.canton)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.provincia)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.provincia)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.direccionExacta)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.direccionExacta)
        </dd>

    </dl>
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>
