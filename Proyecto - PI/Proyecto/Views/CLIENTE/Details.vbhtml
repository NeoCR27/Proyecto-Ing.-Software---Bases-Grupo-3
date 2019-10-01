﻿@ModelType Proyecto.CLIENTE
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

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
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.cedulaPK }) |
    @Html.ActionLink("Back to List", "Index")
</p>