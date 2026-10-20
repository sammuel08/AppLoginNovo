// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//Função Js para mascaras
$(document).ready(function () {
    $('.CEP').mask('00000-000');
    $('.dinheiro').mask('000.000.000.000.000,00', { reverse: true });
    $('.CPF').mask('000.000.000-00', { reverse: true });
    $('.Telefone').mask('(00) 00000-0000');
    $('.Nascimento').mask("00/00/0000", { placeholder: "__/__/____" });
});