$(document).ready(function (e) {
    // Color picker
    const $colorInputs = $("[type=color]");
    if ($colorInputs.length > 0) {
        $colorInputs.each(function (id, el) {
            const $el = $(el);
            $el.hide();
            $el.after('<div class="color-picker"></div>');
            $el.attr("type", "hidden");
            Pickr.create({
                el: $el.next(".color-picker").get(0),
                theme: "nano",
                default: $el.val(),
                components: {
                    preview: true,
                    opacity: true,
                    hue: true,
                    interaction: {
                        hex: false,
                        input: true,
                        clear: false,
                        save: true
                    }
                },
                i18n: {
                    'btn:save': 'Guardar',
                    'btn:cancel': 'Cancelar',
                    'btn:clear': 'Apagar'
                }
            }).on('change', (color, instance) => {
                $el.val(color.toHEXA().toString());
            }).on('save', (color, instance) => {
                instance.hide();
            });
        });
    }
});