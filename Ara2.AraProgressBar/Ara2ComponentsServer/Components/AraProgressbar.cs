// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ara2.Dev;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vBase:true)]
    public class AraProgressbar : AraComponentVisualAnchorConteiner, IAraDev
    {

        public AraProgressbar(IAraObject ConteinerFather)
            : this(AraObjectClienteServer.Create(ConteinerFather, "Div"), ConteinerFather)
        {
            this._MinWidth = 10;
            this._MinHeight = 10;
            this._Width = 100;
            this._Height = 100;
        }

        public AraProgressbar(string vNameObject, IAraObject vConteinerFather)
            : base(vNameObject, vConteinerFather, "AraProgressbar")
        {
            Click = new AraComponentEvent<EventHandler>(this, "Click");
            IsVisible = new AraComponentEvent<EventHandler>(this, "IsVisible");
            this.EventInternal += AraDiv_EventInternal;
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraProgressbar/AraProgressbar.js");
        }

        public void AraDiv_EventInternal(String vFunction)
        {
            switch (vFunction.ToUpper())
            {
                case "CLICK":
                    Click.InvokeEvent(this, new EventArgs());
                    break;
                case "ISVISIBLE":
                    IsVisible.InvokeEvent(this, new EventArgs());
                    break;
            }
        }

        #region Eventos
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Click;

        [AraDevEvent]
        public AraComponentEvent<EventHandler> IsVisible;
        #endregion

        private bool _Enabled = true;

        [AraDevProperty(true)]
        public bool Enabled
        {
            set
            {
                _Enabled = value;
                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetEnabled(" + (_Enabled ? "true" : "false") + "); \n");
            }
            get { return _Enabled; }
        }

        private double? _Value = null;

        [AraDevProperty(null)]
        public double? Value
        {
            set
            {
                _Value = value;
                if (_Value != null && Convert.ToDouble(_Value) > ValueMax)
                    _Value = ValueMax;

                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetValue(" + (_Value != null?_Value.ToString().Replace(".", "").Replace(",", "."):"null") + "); \n");
            }
            get { return _Value; }
        }

        private double _ValueMax = 100;
        [AraDevProperty(100)]
        public double ValueMax
        {
            set
            {
                _ValueMax = value;
                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" vObj.SetValueMax(" + _ValueMax.ToString().Replace(".", "").Replace(",", ".") + "); \n");
            }
            get { return _ValueMax; }
        }
        
        #region Ara2Dev
        private string _Name = "";

        [AraDevProperty]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys =null;
        public AraEvent<DStartEditPropertys> StartEditPropertys 
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }
        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty 
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }
        #endregion
    }
}
