@ModelType IEnumerable(Of Proyecto.CLIENTE)
@Code
ViewData("Title") = "Index"
End Code

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.tel)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.nombre)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.primerApellido)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.segundoApellido)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.correo)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.distrito)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.canton)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.provincia)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.direccionExacta)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.tel)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.nombre)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.primerApellido)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.segundoApellido)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.correo)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.distrito)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.canton)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.provincia)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.direccionExacta)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.cedulaPK }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.cedulaPK }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.cedulaPK })
        </td>
    </tr>
Next

</table>
