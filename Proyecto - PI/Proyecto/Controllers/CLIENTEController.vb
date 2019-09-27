Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports Proyecto

Namespace Controllers
    Public Class CLIENTEController
        Inherits System.Web.Mvc.Controller

        Private db As New Gr03Proy4Entities

        ' GET: CLIENTE
        Function Index() As ActionResult
            Return View(db.CLIENTEs.ToList())
        End Function

        ' GET: CLIENTE/Details/5
        Function Details(ByVal id As String) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim cLIENTE As CLIENTE = db.CLIENTEs.Find(id)
            If IsNothing(cLIENTE) Then
                Return HttpNotFound()
            End If
            Return View(cLIENTE)
        End Function

        ' GET: CLIENTE/Create
        Function Create() As ActionResult
            Return View()
        End Function

        ' POST: CLIENTE/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="cedulaPK,tel,nombre,primerApellido,segundoApellido,correo,distrito,canton,provincia,direccionExacta")> ByVal cLIENTE As CLIENTE) As ActionResult
            If ModelState.IsValid Then
                db.CLIENTEs.Add(cLIENTE)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(cLIENTE)
        End Function

        ' GET: CLIENTE/Edit/5
        Function Edit(ByVal id As String) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim cLIENTE As CLIENTE = db.CLIENTEs.Find(id)
            If IsNothing(cLIENTE) Then
                Return HttpNotFound()
            End If
            Return View(cLIENTE)
        End Function

        ' POST: CLIENTE/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="cedulaPK,tel,nombre,primerApellido,segundoApellido,correo,distrito,canton,provincia,direccionExacta")> ByVal cLIENTE As CLIENTE) As ActionResult
            If ModelState.IsValid Then
                db.Entry(cLIENTE).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(cLIENTE)
        End Function

        ' GET: CLIENTE/Delete/5
        Function Delete(ByVal id As String) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim cLIENTE As CLIENTE = db.CLIENTEs.Find(id)
            If IsNothing(cLIENTE) Then
                Return HttpNotFound()
            End If
            Return View(cLIENTE)
        End Function

        ' POST: CLIENTE/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As String) As ActionResult
            Dim cLIENTE As CLIENTE = db.CLIENTEs.Find(id)
            db.CLIENTEs.Remove(cLIENTE)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
