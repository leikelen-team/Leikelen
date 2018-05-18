var textos = {
	"sections": {
		"main": {
			"es": {
				"name": "Reporte de la Presentacion",
				"explanation": "El presente informe contiene la información sumarizada y resumida de los datos recolectados \
				en la escena"
			}
		},
		"mainPerson": {
			"es": {
				"name": "Reporte de la Persona: ",
				"explanation": "El presente informe contiene la información sumarizada y resumida de los datos recolectados \
				de usted"
			}
		},
		"studentPostures":{
			"es": {
				"name": "Posturas por estudiante",
				"explanation": "A continuacion se muestra la cantidad de tiempo (porcentualmente) de cada postura"
			}
		},
		"voiceLeanNeck":{
			"es": {
				"name": "Voz, Inclinación del cuerpo y orientación del cuello",
				"explanation": "A continuacion se muestra el tiempo que estuvo hablando, la inclinación de su cuerpo y\
				 si estuvo mirando al público"
			}
		},
		"proxemic":{
			"es": {
				"name": "Distancias entre los participantes",
				"explanation": "A continuación se muestran los tipos de distancias entre los participantes usando la \
				proxemia aproximada (distancia entre el esternón de ambas personas)"
			}
		},
		"accproxemic":{
			"es": {
				"name": "",
				"explanation": "A continuación se muestran los tipos de distancias entre los participantes \
				usando la proxemia exacta (distancia mínima entre dos personas, teniendo en cuenta cada parte del cuerpo)"
			}
		},
		"modalInt":{
			"es": {
				"name": "",
				"explanation": ""
			}
		},
		"submodalInt":{
			"es": {
				"name": "",
				"explanation": ""
			}
		},
		"submodalEvents":{
			"es": {
				"name": "Eventos",
				"explanation": "A continuación se muestran los gráficos de eventos por cada tipo y subtipo. \
        Los gráficos contienen 100 o menos puntos (en caso de tener menos de 100) por un tema de visualización y rendimiento, \
        para esto se aplicó un proceso de decimación"
			}
		},
		"treemap": {
			"es": {
				"name": "Mapa de árbol",
				"explanation": ""
			}
		}
	},
	"modaltype": {
		"Discrete Posture":{
			"es": {
				"name": "Posturas",
				"explanation": ""
			},
			"submodaltype": {
				"OpenHands": {
					"es": {
						"name": "Explicando con ambas manos abiertas",
						"explanation": ""
					}
				},
				"HandsDown": {
					"es": {
						"name": "Manos abajo",
						"explanation": ""
					}
				},
				"HandOnHip": {
					"es": {
						"name": "Manos en la cintura",
						"explanation": ""
					}
				},
				"HandOnHead": {
					"es": {
						"name": "Mano en la nuca",
						"explanation": ""
					}
				},
				"Seated": {
					"es": {
						"name": "Sentado",
						"explanation": ""
					}
				},
				"HandOnFace": {
					"es": {
						"name": "Mano en el mentón",
						"explanation": ""
					}
				},
				"CrossArms": {
					"es": {
						"name": "Brazos cruzados",
						"explanation": ""
					}
				},
				"AskingHelp": {
					"es": {
						"name": "Preguntando",
						"explanation": ""
					}
				},
				"OneHand": {
					"es": {
						"name": "Explicando con una mano abierta",
						"explanation": ""
					}
				},
				"Point": {
					"es": {
						"name": "Apuntando",
						"explanation": ""
					}
				}
			}
		},
		"Emotion":{
			"es": {
				"name": "Emoción",
				"explanation": ""
			},
			"submodaltype": {
				"LALV": {
					"es": {
						"name": "Baja agitación, baja valencia",
						"explanation": ""
					}
				},
				"LAHV": {
					"es": {
						"name": "Baja agitación, alta valencia",
						"explanation": ""
					}
				},
				"HALV": {
					"es": {
						"name": "Alta agitación, baja valencia",
						"explanation": ""
					}
				},
				"HAHV": {
					"es": {
						"name": "Alta agitación, alta valencia",
						"explanation": ""
					}
				}
			}
		},
		"Voice":{
			"es": {
				"name": "Voz",
				"explanation": ""
			},
			"submodaltype": {
				"Talked": {
					"es": {
						"name": "Habló",
						"explanation": ""
					}
				}
			}
		},
		"Continuous Posture": {
			"es": {
				"name": "Gestos continuos",
				"explanation": ""
			},
			"submodaltype": {
			}
		},
		"Neck Orientation":{
			"es": {
				"name": "Orientación del cuello",
				"explanation": ""
			},
			"submodaltype": {
				"Pitch" :{
					"es": {
						"name": "Inclinación",
						"explanation": "Rotación en el eje X. Inclinación de la cabeza, en sentido de asentir\
						 (mirar hacia abajo) o  mirar arriba"
					}
				},
				"Watching public" :{
					"es": {
						"name": "Mirando al público",
						"explanation": "Intervalos en los que la persona estuvo mirando al público.\
						 Calculado utilizando la rotación de la cabeza en el eje Y (como mirando de izquierda a derecha),\
						  y triangulando com trigonometría teniendo en cuenta el ancho de la sala y distancia horizontal y\
						   de profundidad con el sensor (tomando el supuesto que el sensor está en la primera fila de alumnos)"
					}
				},
				"Roll" :{
					"es": {
						"name": "Rotación",
						"explanation": "Rotación en el eje Z, inclinando la cabeza hacia uno de los dos hombros."
					}
				},
				"Yaw" :{
					"es": {
						"name": "Ángulo",
						"explanation": "Rotación en el eje Y, como mirando de lado a lado (de izquierda a derecha)."
					}
				}
			}
		},
		"Lean": {
			"es": {
				"name": "Inclinación",
				"explanation": ""
			},
			"submodaltype": {
				"Y": {
					"es": {
						"name": "Horizontal",
						"explanation": "Inclinación horizontal (hacia los lados)"
					}
				},
				"X": {
					"es": {
						"name": "Vertical",
						"explanation": "Inclinación vertical (atrás/arriba o adelante/abajo)."
					}
				},
				"Downside": {
					"es": {
						"name": "Inclinado hacia adelante",
						"explanation": ""
					}
				},
				"Straight": {
					"es": {
						"name": "Pose recta",
						"explanation": ""
					}
				},
				"Upside": {
					"es": {
						"name": "Inclinado hacia atrás",
						"explanation": ""
					}
				}
			}
		},
		"Shoulders":{
			"es": {
				"name": "Hombros",
				"explanation": ""
			},
			"submodaltype": {
				"Angle": {
					"es": {
						"name": "Ángulo entre hombros y cuello",
						"explanation": ""
					}
				}
			}
		},
		"Proxemic":{
			"es": {
				"name": "Proxemia aproximada",
				"explanation": ""
			},
			"submodaltype": {
				"Public": {
					"es": {
						"name": "Distancia pública",
						"explanation": ""
					}
				},
				"Social": {
					"es": {
						"name": "Distancia social",
						"explanation": ""
					}
				},
				"Personal": {
					"es": {
						"name": "Distancia personal",
						"explanation": ""
					}
				},
				"Intimate": {
					"es": {
						"name": "Distancia íntima",
						"explanation": ""
					}
				},
				"Events": {
					"es": {
						"name": "Distancia en el tiempo",
						"explanation": ""
					}
				}
			}
		},
		"AccProxemic":{
			"es": {
				"name": "Proxemia exacta",
				"explanation": ""
			},
			"submodaltype": {
				"Public": {
					"es": {
						"name": "Distancia pública",
						"explanation": ""
					}
				},
				"Social": {
					"es": {
						"name": "Distancia social",
						"explanation": ""
					}
				},
				"Personal": {
					"es": {
						"name": "Distancia personal",
						"explanation": ""
					}
				},
				"Intimate": {
					"es": {
						"name": "Distancia íntima",
						"explanation": ""
					}
				},
				"Events": {
					"es": {
						"name": "Distancia en el tiempo",
						"explanation": ""
					}
				}
			}
		}
	}
};

//getTextTranslated("Lean", null, "es", "name");
//getTextTranslated("Lean", "Y", "es", "explanation");
function getModalSubmodalTextTranslated(modal, submodal, lang, type){
	//console.log(modal + ", "+ submodal+ ", "+lang+ ", "+ type);
	var defaultValue = "";
	if(modal == null || lang == null)
		return defaultValue;
	if(submodal == null){//modal
		if(modal in textos["modaltype"]){
			return textos["modaltype"][modal][lang][type]
		}else{
			return defaultValue;
		}
	}else{//submodal
		if(modal in textos["modaltype"] && submodal in textos["modaltype"][modal]["submodaltype"]){

			return textos["modaltype"][modal]["submodaltype"][submodal][lang][type]
		}else{
			return defaultValue;
		}
	}
}

function getSectionTextTranslated(section, lang, type){
	var defaultValue = "";
	if(section in textos["sections"]){

	
	//console.log(textos["sections"][section][lang][type]);
		return textos["sections"][section][lang][type]
	}else{
		return defaultValue;
	}
}