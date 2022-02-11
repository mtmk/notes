from django.shortcuts import render
from django.http import HttpResponse
from django.views.generic import TemplateView, View

class IndexView (View):
    def get(self, request):
        return HttpResponse('Hi ya class')

def index(request):
    return HttpResponse("Hiya!")

